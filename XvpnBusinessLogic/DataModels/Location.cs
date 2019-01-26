using Prism.Mvvm;
using System.Collections.Generic;

namespace Xvpn.BusinessLogic.DataModels
{
    public class Location : BindableBase
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private int _SortOrder;
        public int SortOrder
        {
            get { return _SortOrder; }
            set
            {
                _SortOrder = value;
                RaisePropertyChanged(nameof(SortOrder));
            }
        }

        private Icon _Icon;
        public Icon Icon
        {
            get { return _Icon; }
            set
            {
                _Icon = value;
                RaisePropertyChanged(nameof(Icon));
            }
        }

        private IEnumerable<Server> _Servers;
        public IEnumerable<Server> Servers
        {
            get { return _Servers; }
            set
            {
                _Servers = value;
                RaisePropertyChanged(nameof(Servers));
            }
        }
    }
}