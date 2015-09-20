using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokenAuthentication.Data.Data;
using TokenBasedAuthentication.Filters;

namespace TokenBasedAuthentication.Controllers
{
    [IdentificationAuthorization]
    public class ProductsController : ApiController
    {
        private IRepository _repo;
        public ProductsController()
        {
            _repo = new ProductRepository();
        }
        public HttpResponseMessage GetProducts()
        {
            var results = _repo.GetProducts();
            int i = results.Count();
            return Request.CreateResponse(HttpStatusCode.Found, results);
        }
        [AllowAnonymous]
        public HttpResponseMessage Index()
        {
            var results = _repo.GetProducts().Where(a => a.Id == 1001);
            int i = results.Count();
            return Request.CreateResponse(HttpStatusCode.Found, results);
        }
     
    }
}
