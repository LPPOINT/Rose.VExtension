using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Rose.VExtension.PluginSystem.Activation.RuntimeActivation;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Reservation;
using Rose.VExtension.PluginSystem.Validation;

namespace Rose.VExtension.PluginSystem.Activation
{

    public class PluginInitializationException : Exception
    {

        public IEnumerable<ActivationStepException> ActivationErrorHandler { get; private set; }

        public PluginInitializationException(IEnumerable<ActivationStepException> activationErrorHandler = null)
        {
            ActivationErrorHandler = activationErrorHandler;
        }

        public PluginInitializationException(string message, IEnumerable<ActivationStepException> activationErrorHandler = null)
            : base(message)
        {
            ActivationErrorHandler = activationErrorHandler;
        }

        public PluginInitializationException(string message, Exception innerException, IEnumerable<ActivationStepException> activationErrorHandler = null)
            : base(message, innerException)
        {
            ActivationErrorHandler = activationErrorHandler;
        }
    }

    public class PluginInitializer : IPluginInitializer
    {
        public PluginInitializer(IPluginsReservationRepository reservationRepository)
        {
            ReservationRepository = reservationRepository;
        }

        public PluginInitializer()
        {
            ReservationRepository = new PluginCollectionReservationRepository();
        }

        public IPluginsReservationRepository ReservationRepository { get; private set; }

        public void InitializePlugin(Plugin plugin, IPluginInitializationHandler handler)
        {

            try
            {
                ActivationInfo activationInfo;

                try
                {

                    if(handler == null) // Хандлер скорее всего не инициализирован, тогда просто создаем локальный экземпляр
                        handler = new PluginInitializationHandler();

                    var kernel = new StandardKernel(new InitializationModule());
                    var validator = kernel.Get<IPluginValidator>();
                    var syntax = kernel.Get<IConfigurationSyntax>();

                    activationInfo = new ActivationInfo(validator, syntax, ReservationRepository, handler);
                }
                catch (Exception e)
                {
                    throw new PluginInitializationException("Произошла ошибка во время сбора данных для активации. Подробнее в InnerException", e);
                }

                var activator = new ActivationStepService();

                activator.AddStepProvider(new PluginCoreStepProvider());
                activator.AddStepProvider(new PluginControllerStepProvider());

                var order = new ActivationOrder(
                    ActivationStepName.NameActivation,
                    ActivationStepName.StorageActivation,
                    ActivationStepName.ReservationActivation,
                    ActivationStepName.SettingsActivation,
                    ActivationStepName.MetaActivation,
                    ActivationStepName.VersionActivation,
                    ActivationStepName.ControllerActivation);


                activator.Activate(plugin, activationInfo, order);

            }
            catch (ActivationStepException e)
            {
                throw new PluginInitializationException(
                    "Активатор не смог активировать один или несколько шагов. Подробнее в InnerException", e);
            }
            catch (PluginInitializationException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new PluginInitializationException("Во время активации плагина возникла непредвиденная ошибка. Подробнее в InnerException", e);
            }


        }

    
    }
}
