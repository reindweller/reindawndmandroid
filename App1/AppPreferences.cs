using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App1
{
    public class AppPreferences
    {
        private ISharedPreferences mSharedPrefs;
        private ISharedPreferencesEditor mPrefsEditor;
        private Context mContext;

        private static String PREFERENCE_USERID_KEY = "PREFERENCE_USERID_KEY";

        public AppPreferences(Context context)
        {
            this.mContext = context;
            mSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            mPrefsEditor = mSharedPrefs.Edit();
        }

        public void saveUserIdKey(string key)
        {
            mPrefsEditor.PutString(PREFERENCE_USERID_KEY, key);
            mPrefsEditor.Commit();
        }

        public string getUserIdKey()
        {
            return mSharedPrefs.GetString(PREFERENCE_USERID_KEY, "");
        }
    }
}