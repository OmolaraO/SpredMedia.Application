using System;
using SpredMedia.UserManagement.Model.Entity;
using SpredMedia.UserManagement.Model.Enum;

namespace SpredMedia.UserManagement.Core.DTOs
{
	public class ProfileRequestDto
	{
        public string? Username { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public DateTimeOffset UpdatedDateTime { get; set; }
        public string? UserId { get; set; }
    }
}

