using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xvpn.BusinessLogic;
using Xvpn.UI.Helper;

namespace Xvpn.UI.LocationModule
{
    public class LocationViewModel : BindableBase, ILocationViewModel
    {
        public LocationViewModel(ILocationService locationService, IExecutePingFactory executePingFactory)
        {
            _LocationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _IExecutePingFactory = executePingFactory ?? throw new ArgumentNullException(nameof(executePingFactory));

            Locations = new ObservableCollection<LocationModel>();

            RefreshCommand = new DelegateCommand(() => Refresh());
            ShowBestLocationCommand = new DelegateCommand(() => ShowBestLocation());

            ShowBestLocationNotificationRequest = new InteractionRequest<INotification>();
            ApiErrorNotificationRequest = new InteractionRequest<INotification>();

            _ButtonText = REFRESH_BUTTON_DEFAULT_VALUE;

            DefaultLocationServiceintervalTimeSpan = TimeSpan.FromMilliseconds(LOCATION_SERVICE_INTERVAL);
        }

        #region INotifyProperties
        private string _ButtonText;
        public string ButtonText
        {
            get { return _ButtonText; }
            private set
            {
                _ButtonText = value;
                RaisePropertyChanged(nameof(ButtonText));
            }
        }

        private ObservableCollection<LocationModel> _Locations;
        public ObservableCollection<LocationModel> Locations
        {
            get { return _Locations; }
            private set
            {
                _Locations = value;
                RaisePropertyChanged(nameof(Locations));
            }
        }

        private LocationPingChanged _CurrentBestLocation;
        public LocationPingChanged CurrentBestLocation
        {
            get { return _CurrentBestLocation; }
            private set
            {
                _CurrentBestLocation = value;
                RaisePropertyChanged(nameof(CurrentBestLocation));
            }
        }
        #endregion

        #region ICommands
        public ICommand RefreshCommand { get; private set; }
        public ICommand ShowBestLocationCommand { get; private set; }
        #endregion

        #region ICommandActions
        private void Refresh()
        {
            RefreshInternal();
        }
        private void ShowBestLocation()
        {
            ShowBestLocationInternal();
        }
        #endregion

        #region InteractionRequests
        public InteractionRequest<INotification> ShowBestLocationNotificationRequest { get; private set; }
        public InteractionRequest<INotification> ApiErrorNotificationRequest { get; private set; }
        #endregion

        #region Methods
        private async void RefreshInternal()
        {
            if (DataRequestInprogress)
            {
                return;
            }

            try
            {
                DataRequestInprogress = true;

                var xvpnLocations = await _LocationService.GetLocations();

                ButtonText = xvpnLocations.ButtonText;

                var locations = xvpnLocations.Locations.SelectMany(
                    x => x.Servers,
                    (y, z) => {

                        var pinger = _IExecutePingFactory.Create();

                        var location = new LocationModel(pinger)
                        {
                            Name = y.Name,
                            Icon = Base64ImageToImage.ToBitmapImage(y.Icon.Value),
                            SortOrder = y.SortOrder,
                            Server = z
                        };

                        location.LocationPingChanged += Location_LocationPingChanged;

                        return location;
                    }
                ).ToList();

                Locations.Clear();
                Locations.AddRange(locations);

                await Task.Delay(DefaultLocationServiceintervalTimeSpan);
            }
            catch (UnauthorizedAccessException)
            {
                RaiseApiErrorInteractionRequest(
                    new Notification()
                    {
                        Title = API_ERROR_TITLE,
                        Content = API_ERROR_403
                    }
                );
            }
            catch (TimeoutException)
            {
                RaiseApiErrorInteractionRequest(
                    new Notification()
                    {
                        Title = API_ERROR_TITLE,
                        Content = API_ERROR_TIME_OUT
                    }
                );
            }
            catch (UnknownApiException)
            {
                RaiseApiErrorInteractionRequest(
                    new Notification()
                    {
                        Title = API_ERROR_TITLE,
                        Content = API_ERROR_UNKNOWN
                    }
                );
            }
            catch (Exception e)
            {
                //- swallowing for now
                //- Probably need to log
                //- out of scope for now
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                DataRequestInprogress = false;
            }
        }
        private void ShowBestLocationInternal()
        {
            ShowBestLocationNotificationRequest.Raise(
                new Notification()
                {
                    Title = "Best Location",
                    Content = this
                }
            );
        }
        private void RaiseApiErrorInteractionRequest(INotification notification)
        {
            ApiErrorNotificationRequest.Raise(notification);
        }
        public void Location_LocationPingChanged(object sender, LocationPingChangedEventArgs e)
        {
            if (CurrentBestLocation == null)
            {
                CurrentBestLocation = e.NewLocationPing;
            }
            else if ((CurrentBestLocation.Server == e.NewLocationPing.Server) && (e.NewLocationPing.Ping == 0))
            {
                CurrentBestLocation = null;
                Debug.WriteLine($"{e.NewLocationPing.LocationName}-{e.NewLocationPing.Server}-{e.NewLocationPing.Ping}");
            }
            else
            {
                if (e.NewLocationPing.Server == CurrentBestLocation.Server)
                {
                    CurrentBestLocation.Ping = e.NewLocationPing.Ping;
                    Debug.WriteLine($"{e.NewLocationPing.LocationName}-{e.NewLocationPing.Server}-{e.NewLocationPing.Ping}");
                }
                else if ((e.NewLocationPing.Ping > 0) && (e.NewLocationPing.Ping < CurrentBestLocation.Ping))
                {
                    CurrentBestLocation = e.NewLocationPing;
                    Debug.WriteLine($"{e.NewLocationPing.LocationName}-{e.NewLocationPing.Server}-{e.NewLocationPing.Ping}");
                }
            }
        }
        #endregion

        public bool DataRequestInprogress { get; private set; } = false;

        private readonly TimeSpan DefaultLocationServiceintervalTimeSpan;
        private ILocationService _LocationService;
        private IExecutePingFactory _IExecutePingFactory;

        public const double LOCATION_SERVICE_INTERVAL = 1000;
        public const string REFRESH_BUTTON_DEFAULT_VALUE = "Refresh";
        public const string API_ERROR_TITLE = "API Error.";
        public const string API_ERROR_403 = "The API returned error code 403.";
        public const string API_ERROR_TIME_OUT = "The request timed out.";
        public const string API_ERROR_UNKNOWN = "An unknown error happened";
    }
}