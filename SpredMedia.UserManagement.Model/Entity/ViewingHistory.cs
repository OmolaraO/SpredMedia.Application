using System;
using SpredMedia.UserManagement.Model.Common;

namespace SpredMedia.UserManagement.Model.Entity
{
	public class ViewingHistory : CommonHistory
	{
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Progress { get; set; }
        public int Ratings { get; set; }
    }
}

