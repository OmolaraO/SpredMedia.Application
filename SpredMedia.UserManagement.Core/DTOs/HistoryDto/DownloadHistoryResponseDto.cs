using System;
using SpredMedia.UserManagement.Model.Enum;

namespace SpredMedia.UserManagement.Core.DTOs.HistoryDto
{
	public class DownloadHistoryResponseDto
	{
        public string? DownloadId { get; set; }
        public string? UserId { get; set; }
        public string? ProfileId { get; set; }
        public string? MovieTitle { get; set; }
        public string? ContentTye { get; set; }
        public string? Duration { get; set; }
        public string? FileFormat { get; set; }
        public string? Size { get; set; }
        public long BitRate { get; set; }
        public Status Status { get; set; }
        public ResolutionType Resolution { get; set; }
        public DateTimeOffset DownloadDate { get; set; }
    }
}

