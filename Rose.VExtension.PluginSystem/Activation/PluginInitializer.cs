using System;
using Ninject;
using Rose.VExtension.PluginSystem.Activation.Platforms;
using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Reservation;
using Rose.VExtension.PluginSystem.Validation;

namespace Rose.VExtension.PluginSystem.Activation
{

    public class PluginInitializationException : Exception
    {
        public PluginInitializationException()
        {
        }

        public PluginInitializationException(string message) : base(message)
        {
        }

        public PluginInitializationException(string message, Exception innerException) : base(message, innerException)
        {
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

        public void InitializePlugin(Plugin plugin)
        {

            try
            {
                ActivationInfo activationInfo;

                try
                {
                    var kernel = new StandardKernel(new InitializationModule());
                    var validator = kernel.Get<IPluginValidator>();
                    var syntax = kernel.Get<IConfigurationSyntax>();

                    activationInfo = new ActivationInfo(validator, syntax, ReservationRepository);
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
