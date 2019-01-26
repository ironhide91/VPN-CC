using Prism.Mvvm;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace Xvpn.UI.Helper
{
    public class ExecutePingAsync : BindableBase, IExecutePing
    {
        public ExecutePingAsync(IPing ping)
        {
            _Ping = ping ?? throw new ArgumentNullException(nameof(ping));

            DefaultPingIntervalTimeSpan = TimeSpan.FromMilliseconds(DEFAULT_PING_INTERVAL);

            NewPingNeeded += ExecutePingUsingTask_NewPingNeeded;
        }

        private void ExecutePingUsingTask_NewPingNeeded(object sender, EventArgs e)
        {
            ExecuteInternal();
        }

        public void Execute()
        {
            NewPingNeeded.Invoke(this, EventArgs.Empty);
        }

        public IPAddress IPAddress { get; set; }

        public double PingInterval { get; set; }

        public long _RoundtripTime;
        public long RoundtripTime
        {
            get { return _RoundtripTime; }
            private set
            {
                _RoundtripTime = value;
                RaisePropertyChanged(nameof(RoundtripTime));
            }
        }

        public event EventHandler<PingChangedEventArgs> PingChanged;

        private async void ExecuteInternal()
        {
            RoundtripTime = await _Ping.Ping(IPAddress);

            var newPing = new PingChanged();
            newPing.Server = IPAddress;
            newPing.Ping = RoundtripTime;

            var args = new PingChangedEventArgs();
            args.NewPing = newPing;

            PingChanged?.Invoke(this, args);

            await Task.Delay(DefaultPingIntervalTimeSpan);

            NewPingNeeded.Invoke(this, EventArgs.Empty);
        }

        private IPing _Ping;

        private event EventHandler<EventArgs> NewPingNeeded;

        private readonly TimeSpan DefaultPingIntervalTimeSpan;

        private const double DEFAULT_PING_INTERVAL = 1000;
    }
}