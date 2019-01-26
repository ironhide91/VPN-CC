using Prism.Mvvm;

namespace Xvpn.BusinessLogic.DataModels
{
    public class Icon : BindableBase
    {
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        private byte[] _Value;
        public byte[] Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}
