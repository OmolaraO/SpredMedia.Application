using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.Authentication.Core.DTO
{
    public class ClientResponseDto
    {
        public string ClientId { get; set; }
        public string ClientUsername { get; set; }
        public string ClientSecret { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
    }
}
