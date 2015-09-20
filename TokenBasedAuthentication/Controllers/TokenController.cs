using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using TokenAuthentication.Data.Data;
using TokenAuthentication.Data.Entities;
using TokenBasedAuthentication.Filters;
using TokenBasedAuthentication.Models;

namespace TokenBasedAuthentication.Controllers
{
    public class TokenController : ApiController
    {
        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh";
        
        private IRepository _repo;
        public TokenController()
        {
            _repo = new ProductRepository();
        }
        [HttpPost]
        public HttpResponseMessage Post([FromBody]TokenRequestModel model)
        {
            try
            {
                var user = _repo.GetApiUsers().Where(app => app.AppId == model.ApiKey).FirstOrDefault();
                if (user == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
                }
                else
                {
                    if (isSignatureValid(model,user.Secret))
                    {
                        string secret = GetToken(user.Secret);
                        ApiToken apiToken = new ApiToken() { ExpirationTime = System.DateTime.Now.AddMinutes(30), Token = secret, UserId = user.Id };
                        _repo.AddToken(apiToken);
                        return Request.CreateResponse(HttpStatusCode.OK, secret);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Mismatch");
                }
            }
            catch (System.Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.ToString());
            }
        }

        private bool isSignatureValid(TokenRequestModel model,string userSecret)
        {
            string secret = string.Empty;
            using (HMAC hmac=HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                var hash1 = hmac.ComputeHash(Encoding.UTF8.GetBytes(_salt));
                var hash2 = hmac.ComputeHash(Encoding.UTF8.GetBytes(userSecret));
                var hash3 = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.ApiKey));
                int i = 0;
                byte c;
                var signature = "";
                foreach (byte b in hash2)
                {

                    c = hash1[i];
                    signature = signature + (b & hash1[i] ^ hash3[i]);
                    i++;
                }
                secret = Convert.ToBase64String(Encoding.UTF8.GetBytes(signature));
                
            }
            if (secret.CompareTo(model.Signature) == 0)
                return true;
            else
                return false;
        }

        

        private string GetToken(string userSecret)
        {
            string preToken = Guid.NewGuid().ToString().Replace("-", "");

            var secret = userSecret;
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(preToken);
                var hash1 = hmac.ComputeHash(Encoding.UTF8.GetBytes(preToken));
                var hash2 = hmac.ComputeHash(Encoding.UTF8.GetBytes(secret));
                int i = 0;
                byte c;
                var token = "";
                foreach (byte b in hash2)
                {

                    c = hash1[i];
                    token = token + (b & c);
                    i++;
                }
                secret = Convert.ToBase64String(hmac.Hash);
            }
            return secret;
        }
    }
}
