using Prism.Mvvm;
using System.Collections.Generic;

namespace Vpn.BusinessLogic.DataModels
{
    public class VpnLocations : BindableBase
    {
        private IEnumerable<Location> _Locations;
        public IEnumerable<Location> Locations
        {
            get { return _Locations; }
            set
            {
                _Locations = value;
                RaisePropertyChanged(nameof(Locations));
            }
        }

        private string _ButtonText;
        public string ButtonText
        {
            get { return _ButtonText; }
            set
            {
                _ButtonText = value;
                RaisePropertyChanged(nameof(ButtonText));
            }
        }
    }
}
