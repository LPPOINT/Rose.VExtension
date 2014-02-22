using Rose.VExtension.PluginSystem.Configuration;
using Rose.VExtension.PluginSystem.Reservation;
using Rose.VExtension.PluginSystem.Validation;

namespace Rose.VExtension.PluginSystem.Activation
{
    public class ActivationInfo
    {

        public ActivationInfo()
        {
            
        }

        public ActivationInfo(IPluginValidator validator, IConfigurationSyntax syntax, IPluginsReservationRepository reservationRepository, IPluginInitializationHandler initializationHandler)
        {
            Validator = validator;
            Syntax = syntax;
            ReservationRepository = reservationRepository;
            ActivationHandler = initializationHandler;
        }

        public IPluginValidator Validator { get; set; }
        public IConfigurationSyntax Syntax { get; set; }
        public IPluginsReservationRepository ReservationRepository { get; set; }
        public IPluginInitializationHandler ActivationHandler { get; set; }
    }
}
