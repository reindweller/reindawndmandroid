using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using App1.Enum;
using App1.Model;
using App1.Service;
using Newtonsoft.Json;

namespace App1
{

    [Activity(Label = "Map")]
    public class MapActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        private GoogleMap mMap;
        private Button btnNormal;
        private Button btnHybrid;
        private Button btnSatellite;
        private Button btnTerrain;
        private Button btnDistress;
        private EditText txtDescription;
        private Button btnSubmitDistress;
        private Button btnCancel;
        private LinearLayout layMapAction;
        private LinearLayout layDescription;

        private LatLng pos;
        private Context _mContext;
        private AppPreferences _ap;

        private Location _currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;
        readonly string[] PermissionsLocation =
   {
      Manifest.Permission.AccessCoarseLocation,
      Manifest.Permission.AccessFineLocation
    };

        const int RequestLocationId = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Map);
            // Create your application here

            //btnNormal = FindViewById<Button>(Resource.Id.btnNormal)
            btnDistress = FindViewById<Button>(Resource.Id.btnDistress);
            btnDistress.Click += BtnDistress_Click;

            btnSubmitDistress = FindViewById<Button>(Resource.Id.btnSubmitDistress);
            btnSubmitDistress.Click += BtnSubmitDistress_Click;

            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Click += BtnCancel_Click;

            txtDescription = FindViewById<EditText>(Resource.Id.txtDescription);

            layMapAction = FindViewById<LinearLayout>(Resource.Id.layMapAction);
            layDescription = FindViewById<LinearLayout>(Resource.Id.layDescription);


            InitializeLocationManager();
            if (_currentLocation == null)
            {
                Toast.MakeText(this.ApplicationContext, "Can't determine the current address. Try again in a few minutes.", ToastLength.Short).Show();
            }
            SetUpMap();
        }


        async void GetCurrentAddress()
        {
            if (_currentLocation == null)
            {
                Toast.MakeText(this.ApplicationContext, "Can't determine the current address. Try again in a few minutes.", ToastLength.Short).Show();
                return;
            }

            Address address = await ReverseGeocodeCurrentLocation();
            pos = new LatLng(address.Latitude, address.Longitude);
        }

        private void InitializeLocationManager()
        {

            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessFineLocation) ==
                Permission.Granted &&
                ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.AccessCoarseLocation) ==
                Permission.Granted)
            {

                _locationManager = (LocationManager)GetSystemService(LocationService);
                Criteria criteriaForLocationService = new Criteria
                {
                    Accuracy = Accuracy.Fine
                };
                IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

                if (acceptableLocationProviders.Any())
                {
                    _locationProvider = acceptableLocationProviders.First();
                }
                else
                {
                    _locationProvider = string.Empty;
                }
                //Log.Debug(TAG, "Using " + _locationProvider + ".");

            }
            else
            {
                CallPermission();
            }
            
        }


        private async void CallPermission()
        {
            await GetLocationPermissionAsync();
        }

        async Task GetLocationPermissionAsync()
        {
            //Check to see if any permission in our group is available, if one, then all are
            const string permission = Manifest.Permission.AccessFineLocation;
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                //await GetLocationAsync();
                return;
            }

            //need to request permission
            if (ShouldShowRequestPermissionRationale(permission))
            {
                //Explain to the user why we need to read the contacts
                //Snackbar.Make(layout, "Location access is required to show coffee shops nearby.", Snackbar.LengthIndefinite)
                //        .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
                //        .Show();
                Toast.MakeText(this.ApplicationContext, "Please grant permission", ToastLength.Short).Show();
                return;
            }
            //Finally request permissions with the list of permissions and Id
            RequestPermissions(PermissionsLocation, RequestLocationId);
        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            //Permission granted
                            //var snack = Snackbar.Make(layout, "Location permission is available, getting lat/long.", Snackbar.LengthShort);
                            //snack.Show();

                            //await GetLocationAsync();
                            InitializeLocationManager();
                            Toast.MakeText(this.ApplicationContext, "Location permission is available, getting lat/long.", ToastLength.Short).Show();
                        }
                        else
                        {
                            //Permission Denied :(
                            //Disabling location functionality
                            //var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
                            //snack.Show();
                            Toast.MakeText(this.ApplicationContext, "Location permission is denied.", ToastLength.Short).Show();
                        }
                    }
                    break;
            }
        }
        

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            layMapAction.Visibility = ViewStates.Visible;
            layDescription.Visibility = ViewStates.Gone;
        }

        private void BtnSubmitDistress_Click(object sender, EventArgs e)
        {
            _mContext = Android.App.Application.Context;
            _ap = new AppPreferences(_mContext);
            string key = _ap.getUserIdKey();

            if (string.IsNullOrEmpty(key))
            {
                Toast.MakeText(this.ApplicationContext, "Please login", ToastLength.Short).Show();
                StartActivity(typeof(LoginActivity));
            }

            var model = new DisasterLocationModel
            {
                UserId = new Guid(key),
                Lat = Convert.ToDecimal(pos.Latitude),
                Lng = Convert.ToDecimal(pos.Longitude),
                Description = txtDescription.Text,
                Status = DisasterLocationStatusEnum.Unresponded,
                DatePosted = DateTime.Now
            };

            var result = ApiCallService.CreateRequest(model, Constants.ApiUrl + "disasterlocation/adddisaster");
            using (var reader = new StreamReader(result.GetResponseStream()))
            {
                string responseStr = reader.ReadToEnd();
                //var user = JsonConvert.DeserializeObject<UserModel>(responseStr);

                Toast.MakeText(this.ApplicationContext, "Distress sent", ToastLength.Short).Show();
                StartActivity(typeof(DistressActivity));
            }
        }

        private void BtnDistress_Click(object sender, EventArgs e)
        {
            layMapAction.Visibility = ViewStates.Gone;
            layDescription.Visibility = ViewStates.Visible;
        }

        private void SetUpMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
                //mapView.GetMapAsync(this);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            GetCurrentAddress();

            mMap = googleMap;

            
            string latStr = Intent.GetStringExtra("lat") ?? "";
            string lngStr = Intent.GetStringExtra("lng") ?? "";

            LatLng latlng;
            if (string.IsNullOrEmpty(latStr) || string.IsNullOrEmpty(lngStr))
            {
                latlng = pos;
            }
            else
            {
                latlng = new LatLng(Convert.ToDouble(latStr), Convert.ToDouble(lngStr));
            }

            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 10);
            mMap.MoveCamera(camera);
            MarkerOptions options = new MarkerOptions()
                .SetPosition(latlng)
                .SetTitle("New York")
                .SetSnippet("AKA: The big apple")
                .Draggable(true);

            mMap.AddMarker(options);
            pos = latlng;
            mMap.MarkerDragEnd += MMap_MarkerDragEnd;
        }

        private void MMap_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            pos = e.Marker.Position;

        }

        public void OnLocationChanged(Location location)
        {
            throw new NotImplementedException();
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        protected override void OnResume()
        {
            //TODO: add permission check here
            base.OnResume();
            //_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            //TODO: add permission check here
            base.OnPause();
            //_locationManager.RemoveUpdates(this);
        }

        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }
    }
}