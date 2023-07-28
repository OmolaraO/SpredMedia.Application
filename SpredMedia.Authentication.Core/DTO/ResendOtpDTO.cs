using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.DTO
{
    public class ResendOtpDTO
    {
        public string Email { get; set; }
        public string Purpose { get; set; }
        public string Template { get; set; }
    }
}
