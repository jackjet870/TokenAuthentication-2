using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenAuthentication.Data.Entities;


namespace TokenAuthentication.Data.Data
{
    public class ProductContext:DbContext
    {
        public ProductContext():base("ProductContext")
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApiUser> ApiUsers { get; set; }
        public DbSet<ApiToken> ApiTokens { get; set; }
    }
}
