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
using App1.Adapters;
using App1.Model;
using App1.Service;
using Newtonsoft.Json;

namespace App1
{
    [Activity(Label = "NewsActivity")]
    public class NewsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.News);

            Button newNewsNav = FindViewById<Button>(Resource.Id.newNewsNav);
            newNewsNav.Click += NewNewsNav_Click;

            ListView newsListView = FindViewById<ListView>(Resource.Id.newsListView);
            newsListView.ChoiceMode = ChoiceMode.Single;
            newsListView.Adapter = new NewsAdapter(this, GetNews());
        }

        private void NewNewsNav_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(NewNewsActivity));
        }

        

        private List<NewsModel> GetNews()
        {
            var response = ApiCallService.GetAllRequest(Constants.ApiUrl + "news");

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                //Stream responseStream = response.GetResponseStream();
                string responseStr = reader.ReadToEnd();
                var modelList = JsonConvert.DeserializeObject<IEnumerable<NewsModel>>(responseStr);
                return modelList.ToList();
            }
            
        } 
    }
}