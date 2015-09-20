using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenAuthentication.Data.Entities;

namespace TokenAuthentication.Data.Data
{
    public class ProductRepository:IRepository
    {
        private ProductContext _ctx;
        public ProductRepository()
        {
            _ctx = new ProductContext();
        }
        public IQueryable<Product> GetProducts()
        {
            return _ctx.Products;
        }


        public IQueryable<ApiUser> GetApiUsers()
        {
            return _ctx.ApiUsers;
        }


        public bool AddToken(ApiToken apiToken)
        {
            _ctx.ApiTokens.Add(apiToken);
            return _ctx.SaveChanges() > 0 ? true : false;

        }


        public bool ValidateToken(string apiKey, string token)
        {
            var user=_ctx.ApiUsers.Where(a => a.AppId == apiKey).Select(a => new { id=a.Id}).FirstOrDefault();
            
            var results=_ctx.ApiTokens.Where(a => a.Token == token && a.UserId==user.id );
            int id = 0;
            foreach (var v in results)
            {
                TimeSpan diff = v.ExpirationTime - DateTime.Now;
                int i = (int)diff.Minutes ;
                if(i<30)
                {
                    id = v.Id;
                }
               
            }
            if (id > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}