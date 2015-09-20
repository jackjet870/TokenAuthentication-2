using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TokenAuthentication.Data.Data;

namespace TokenBasedAuthentication.Models
{
    public class TokenUserManager
    {
        private IRepository _repo;
        public string ApiKey { get; set; }
        public string Token { get; set; }
        public TokenUserManager( )
        {
            _repo = new ProductRepository();
        }
        public bool ValidateUser()
        {
            return _repo.ValidateToken(ApiKey, Token);
        }

    }
}