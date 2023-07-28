using System;
using System.Net;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Model.Common;
using SpredMedia.UserManagement.Model.Enum;

namespace SpredMedia.UserManagement.Model.Entity
{
	public class DownloadHistory : CommonHistory
	{
		public string? FileFormat { get; set; }
		public string? Size { get; set; }
		public long BitRate { get; set; }
		public Status Status { get; set; }
		public ResolutionType Resolution { get; set; }
		public DateTimeOffset DownloadDate { get; set; }
		public IEnumerable<Profile> profiles { get; set; }
	}
}

