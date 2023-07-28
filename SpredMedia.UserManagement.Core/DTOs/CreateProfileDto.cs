using System;
using SpredMedia.UserManagement.Model.Enum;

namespace SpredMedia.UserManagement.Core.DTOs
{
	public class CreateProfileDto
	{                           
        public string? Username { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTimeOffset CreationDate { get; set; }
        public string? UserId { get; set; }
    }
}

