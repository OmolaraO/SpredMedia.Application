using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.DTO
{
    public class ConfirmEmailDTO
    {
        public string EmailAddress { get; set; }
        public string Token { get; set; }
    }
}
