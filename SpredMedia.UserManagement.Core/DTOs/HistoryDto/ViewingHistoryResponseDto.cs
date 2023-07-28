using System;
using SpredMedia.UserManagement.Model.Enum;

namespace SpredMedia.UserManagement.Core.DTOs.HistoryDto
{
	public class ViewingHistoryResponseDto
	{
        public string? ViewId { get; set; }
        public string? UserId { get; set; }
        public string? ProfileId { get; set; }
        public string? MovieTitle { get; set; }
        public string? ContentTye { get; set; }
        public string? Duration { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }
        public string? Progress { get; set; }
        public int Ratings { get; set; }
    }
}

