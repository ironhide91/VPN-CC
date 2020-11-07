using Prism.Mvvm;
using System.Net;

namespace Vpn.BusinessLogic.DataModels
{
    public class Server : BindableBase
    {
        private IPAddress _IP;
        public IPAddress IP
        {
            get { return _IP; }
            set
            {
                _IP = value;
                RaisePropertyChanged(nameof(IP));
            }
        }
    }
}
