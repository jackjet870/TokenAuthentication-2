using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenAuthentication.Data.Entities
{
    public class ApiToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
        public int UserId { get; set; }
    }
}
