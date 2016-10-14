using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App1.Enum
{
    public enum DisasterLocationStatusEnum
    {
        [Description("Unresponded")]
        Unresponded = 0,

        [Description("Responding")]
        Responding = 1,

        [Description("Responded")]
        Responded = 2,

        [Description("Cancelled")]
        Cancelled = 3
    }
}