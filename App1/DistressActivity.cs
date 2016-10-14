using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App1.Adapters;
using App1.Model;
using App1.Service;
using Newtonsoft.Json;

namespace App1
{
    [Activity(Label = "DistressActivity")]
    public class DistressActivity : Activity
    {

        private Context _mContext;
        private AppPreferences _ap;
        private string _userId;
        private ListView _disressListView;

        private Button _btnAllDistress;
        private Button _btnMyResponse;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Distress);
            // Create your application here

            _mContext = Android.App.Application.Context;
            _ap = new AppPreferences(_mContext);
            _userId = _ap.getUserIdKey();

            _disressListView = FindViewById<ListView>(Resource.Id.disressListView);
            _disressListView.ChoiceMode = ChoiceMode.Single;
            _disressListView.Adapter = new DistressAdapter(this, GetDistress());

            
            RegisterForContextMenu(_disressListView);
            _disressListView.ItemClick += DisressListView_ItemClick;

            _btnAllDistress = FindViewById<Button>(Resource.Id.btnAllDistress);
            _btnMyResponse = FindViewById<Button>(Resource.Id.btnMyResponse);

            _btnAllDistress.Click += _btnAllDistress_Click;
            _btnMyResponse.Click += _btnMyResponse_Click;
        }

        private void _btnMyResponse_Click(object sender, EventArgs e)
        {
            _disressListView.Adapter = new DistressAdapter(this, GetMyResponses());
        }

        private void _btnAllDistress_Click(object sender, EventArgs e)
        {
            _disressListView.Adapter = new DistressAdapter(this, GetDistress());
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            MenuInflater.Inflate(Resource.Menu.DistressMenu, menu);

            base.OnCreateContextMenu(menu, v, menuInfo);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var model = Cast(_disressListView.GetItemAtPosition(info.Position));

            switch (item.ItemId)
            {
                case Resource.Id.distressRespond:
                    Toast.MakeText(this.ApplicationContext, "Respond", ToastLength.Short).Show();
                    RespondDisaster(model.Id);
                    return true;
                case Resource.Id.distressViewMap:
                    var activity2 = new Intent(this, typeof(MapActivity));
                    activity2.PutExtra("lat", model.Lat.ToString(CultureInfo.CurrentCulture));
                    activity2.PutExtra("lng", model.Lng.ToString(CultureInfo.CurrentCulture));
                    StartActivity(activity2);
                    return true;
                case Resource.Id.distressCancel:
                    Toast.MakeText(this.ApplicationContext, "Cancel", ToastLength.Short).Show();
                    return true;
            }
            return base.OnMenuItemSelected(featureId, item);
        }
        
        public static DisasterLocationModel Cast(Object obj)
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as DisasterLocationModel;
        }


        private void DisressListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //e.View.CreateContextMenu(new ContextMenu);
        }

        private List<DisasterLocationModel> GetDistress()
        {
            var response = ApiCallService.GetAllRequest(Constants.ApiUrl + "disasterlocation");

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                //Stream responseStream = response.GetResponseStream();
                string responseStr = reader.ReadToEnd();
                var modelList = JsonConvert.DeserializeObject<IEnumerable<DisasterLocationModel>>(responseStr);
                return modelList.ToList();
            }

        }

        private List<DisasterLocationModel> GetMyResponses()
        {
            if (string.IsNullOrEmpty(_userId))
            {
                Console.WriteLine("please login");
                StartActivity(typeof(LoginActivity));
            }

            var userid = new Guid(_userId);
            var result = ApiCallService.CreateRequest(userid, Constants.ApiUrl + "disasterlocation/getMyResponses");

            // Will block until the task is completed...
            //HttpWebResponse result = response.GetAwaiter().GetResult();
            using (var reader = new StreamReader(result.GetResponseStream()))
            {
                string responseStr = reader.ReadToEnd();
                var modelList = JsonConvert.DeserializeObject<IEnumerable<DisasterLocationModel>>(responseStr);
                return modelList.ToList();
            }


            //var response = ApiCallService.GetAllRequest(Constants.ApiUrl + "getMyResponses");

            //using (var reader = new StreamReader(response.GetResponseStream()))
            //{
            //    //Stream responseStream = response.GetResponseStream();
            //    string responseStr = reader.ReadToEnd();
            //    var modelList = JsonConvert.DeserializeObject<IEnumerable<DisasterLocationModel>>(responseStr);
            //    return modelList.ToList();
            //}

        }

        public void RespondDisaster(Guid distressId)
        {
             
            if (string.IsNullOrEmpty(_userId))
            {
                Console.WriteLine("please login");
                StartActivity(typeof(LoginActivity));
                return;
            }
            
            var model = new DisasterLocationModel
            {
                Id = distressId,
                RespondentId = new Guid(_userId)
            };


            var result = ApiCallService.CreateRequest(model, Constants.ApiUrl + "disasterlocation/responddisaster");
            using (var reader = new StreamReader(result.GetResponseStream()))
            {
                string responseStr = reader.ReadToEnd();
                if (responseStr == String.Empty)
                {
                    Toast.MakeText(this.ApplicationContext, "response failed!", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this.ApplicationContext, "distress is now for response", ToastLength.Short).Show();
                    _disressListView.Adapter = new DistressAdapter(this, GetDistress());
                    return;
                }
            }
        }
    }
}