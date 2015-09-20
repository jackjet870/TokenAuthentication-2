using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenAuthentication.Data.Entities;

namespace TokenAuthentication.Data.Data
{
    public interface IRepository
    {
        IQueryable<Product> GetProducts();
        IQueryable<ApiUser> GetApiUsers();
        bool AddToken(ApiToken apiToken);
        bool ValidateToken(string apiKey, string token);
    }
}
