using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App1.Model;
using App1.Service;
using Newtonsoft.Json;


namespace App1
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            // Create your application here

            Button loginSbmit = FindViewById<Button>(Resource.Id.loginSbmit);
            loginSbmit.Click += LoginSbmit_Click;
        }

        private void LoginSbmit_Click(object sender, EventArgs e)
        {
            EditText emailTxt = FindViewById<EditText>(Resource.Id.emailTxt);
            EditText passwordTxt = FindViewById<EditText>(Resource.Id.passwordTxt);

            var userlogin = new LoginModel
            {
                Email = emailTxt.Text,
                Password = passwordTxt.Text
            };

            //var response = ApiCallService.CreateRequest(userlogin, Constants.ApiUrl + "user/login");
            var result = ApiCallService.CreateRequest(userlogin, Constants.ApiUrl + "user/login");

            // Will block until the task is completed...
            //HttpWebResponse result = response.GetAwaiter().GetResult();
            using (var reader = new StreamReader(result.GetResponseStream()))
            {
                //Stream responseStream = response.GetResponseStream();
                //string responseStr = reader.ReadToEnd();
                //Console.WriteLine(responseStr);
                ////return XmlUtils.Deserialize<TResponse>(reader);

                string responseStr = reader.ReadToEnd();
                var user = JsonConvert.DeserializeObject<UserModel>(responseStr);

                Context mContext = Application.Context;
                AppPreferences ap = new AppPreferences(mContext);

                //string key = "123123";
                ap.saveUserIdKey(user.Id.ToString());

                StartActivity(typeof(MainActivity));
            }
        }
    }
}