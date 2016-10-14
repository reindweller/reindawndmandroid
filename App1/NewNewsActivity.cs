using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App1.Model;
using App1.Service;
using Newtonsoft.Json;

namespace App1
{
    [Activity(Label = "NewNewsActivity")]
    public class NewNewsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NewNews);

            Button postSubmit = FindViewById<Button>(Resource.Id.postSubmit);
            postSubmit.Click += PostSubmit_Click;
        }

        private void PostSubmit_Click(object sender, EventArgs e)
        {
            EditText titleTxt = FindViewById<EditText>(Resource.Id.titleTxt);
            EditText messageTxt = FindViewById<EditText>(Resource.Id.messageTxt);

            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);
            string key = ap.getUserIdKey();
            Guid userId = Guid.Empty;
            try
            {
                userId = new Guid(key);
            }
            catch (Exception)
            {
                Toast.MakeText(this.ApplicationContext, "Please login", ToastLength.Short).Show();
            }

            var model = new NewsModel
            {
                UserId = userId,
                Title = titleTxt.Text,
                Message = messageTxt.Text,
                DatePosted = DateTime.Now
            };
            
            var result = ApiCallService.CreateRequest(model, Constants.ApiUrl + "news/postnews");
            using (var reader = new StreamReader(result.GetResponseStream()))
            {
                string responseStr = reader.ReadToEnd();
                if (responseStr == String.Empty)
                {
                    Toast.MakeText(this.ApplicationContext, "Post failed!", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this.ApplicationContext, "Post success!", ToastLength.Short).Show();
                    StartActivity(typeof(NewsActivity));
                }
            }
        }
        
    }
}