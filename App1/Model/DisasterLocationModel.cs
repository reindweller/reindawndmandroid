using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App1.Enum;

namespace App1.Model
{
    public class DisasterLocationModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? RespondentId { get; set; } //UserId
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string Description { get; set; }
        public DisasterLocationStatusEnum Status { get; set; }
        public DateTime DatePosted { get; set; }
    }

    public class DisasterLocationUpdateModel
    {
        public Guid Id { get; set; }
        public Guid RespondentId { get; set; } //UserId
    }


}