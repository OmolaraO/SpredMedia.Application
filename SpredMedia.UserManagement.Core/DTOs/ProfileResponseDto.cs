using System;
using SpredMedia.UserManagement.Model.Entity;
using SpredMedia.UserManagement.Model.Enum;

namespace SpredMedia.UserManagement.Core.DTOs
{
	public class ProfileResponseDto
	{
        public string? Username { get; set; }
        public string? Address { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public DateTimeOffset UpdatedDateTime { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string? UserId { get; set; }
        public IEnumerable<DownloadHistory>? DownloadHistories { get; set; }
        public IEnumerable<ViewingHistory>? ViewingHistories { get; set; }
    }
}

