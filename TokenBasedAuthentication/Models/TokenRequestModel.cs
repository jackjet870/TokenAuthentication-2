using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenBasedAuthentication.Models
{
    public class TokenRequestModel
    {
        public string ApiKey { get; set; }
        public string Signature { get; set; }
    }
}