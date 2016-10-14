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
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            // Create your application here
            Button registerBtn = FindViewById<Button>(Resource.Id.registerBtn);
            registerBtn.Click += RegisterBtn_Click;
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            EditText emailTxt = FindViewById<EditText>(Resource.Id.emailTxt);
            EditText passwordTxt = FindViewById<EditText>(Resource.Id.passwordTxt);
            EditText confirmPasswordTxt = FindViewById<EditText>(Resource.Id.confirmPasswordTxt);
            RadioButton rdo1 = FindViewById<RadioButton>(Resource.Id.RescuerRdo);
            RadioButton rdo2 = FindViewById<RadioButton>(Resource.Id.VictimRdo);

            var model = new RegisterViewModel
            {
                Email = emailTxt.Text,
                Password = passwordTxt.Text,
                ConfirmPassword = confirmPasswordTxt.Text,
                Role = rdo1.Checked? "admin" : "user"
            };

            var result = ApiCallService.CreateRequest(model, Constants.ApiUrl + "user/register");
            using (var reader = new StreamReader(result.GetResponseStream()))
            {
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