using System;
using System.IO;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Views;
using Java.Lang.Reflect;
using Newtonsoft.Json;
using AndroidUri = Android.Net.Uri;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Context _mContext;
        private AppPreferences _ap;
        private Button _loginNavButton;
        private Button _registerNavButton;
        private Button _newsNavButton;
        private Button _mapNavButton;
        private Button _logOutNavButton;
        private Button _distressNavButton;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            _mContext = Android.App.Application.Context;
            _ap = new AppPreferences(_mContext);
            string key = _ap.getUserIdKey();

            _loginNavButton = FindViewById<Button>(Resource.Id.loginNav);
            _registerNavButton = FindViewById<Button>(Resource.Id.registerNav);
            _newsNavButton = FindViewById<Button>(Resource.Id.newsNav);
            _mapNavButton = FindViewById<Button>(Resource.Id.mapNav);
            _logOutNavButton = FindViewById<Button>(Resource.Id.logoutNav);
            _distressNavButton = FindViewById<Button>(Resource.Id.distressNav);

            if (string.IsNullOrEmpty(key))
            {

                _loginNavButton.Visibility = ViewStates.Visible;
                _registerNavButton.Visibility = ViewStates.Visible;
                _newsNavButton.Visibility = ViewStates.Gone;
                _mapNavButton.Visibility = ViewStates.Gone;
                _distressNavButton.Visibility = ViewStates.Gone;
                _logOutNavButton.Visibility = ViewStates.Gone;
            }
            else
            {
                _loginNavButton.Visibility = ViewStates.Gone;
                _registerNavButton.Visibility = ViewStates.Gone;
                _newsNavButton.Visibility = ViewStates.Visible;
                _mapNavButton.Visibility = ViewStates.Visible;
                _distressNavButton.Visibility = ViewStates.Visible;
                _logOutNavButton.Visibility = ViewStates.Visible;
            }

            
            _loginNavButton.Click += LoginNavButton_Click;
            _registerNavButton.Click += RegisterNavButton_Click;
            _newsNavButton.Click += NewsNavButton_Click;
            _mapNavButton.Click += MapNavButton_Click;
            _distressNavButton.Click += _distressNavButton_Click;
            _logOutNavButton.Click += LogOutNavButton_Click;

        }

        private void _distressNavButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(DistressActivity));
        }

        private void LogOutNavButton_Click(object sender, EventArgs e)
        {
            _ap.saveUserIdKey(string.Empty);
            _loginNavButton.Visibility = ViewStates.Visible;
            _registerNavButton.Visibility = ViewStates.Visible;
            _newsNavButton.Visibility = ViewStates.Gone;
            _mapNavButton.Visibility = ViewStates.Gone;
            _logOutNavButton.Visibility = ViewStates.Gone;
        }

        private void NewsNavButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(NewsActivity));
        }

        private void RegisterNavButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }

        private void MapNavButton_Click(object sender, EventArgs e)
        {
            //AndroidUri geoUri = AndroidUri.Parse("geo:42.374260,-71.120824");
            //Intent mapIntent = new Intent(Intent.ActionView, geoUri);
            //StartActivity(mapIntent);
            StartActivity(typeof(MapActivity));
        }

        private void LoginNavButton_Click(object sender, EventArgs e)
        {
            //var intent = new Intent(this, typeof(LoginActivity));
            //intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
            StartActivity(typeof(LoginActivity));
        }

    }
    
}

