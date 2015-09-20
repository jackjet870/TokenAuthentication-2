using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Text;
using System.Security.Principal;
using System.Threading;
using TokenBasedAuthentication.Models;

namespace TokenBasedAuthentication.Filters
{
    public class IdentificationAuthorization: AuthorizationFilterAttribute 
    {
        const string APIKEYNAME = "apikey";
        const string TOKENNAME = "token";
        public override void OnAuthorization(HttpActionContext actionContext)
        {
           var query= HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
           string apiKey = query[APIKEYNAME];
           string token = query[TOKENNAME];
            if(!string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(token))
            {
                TokenUserManager tokenManager = new TokenUserManager();
                tokenManager.ApiKey = apiKey;
                tokenManager.Token = token;
                if (tokenManager.ValidateUser())
                {
                    return;
                }
                
                    
            }
            RespondUnAuthorize(actionContext);
        }
        private void RespondUnAuthorize(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

            actionContext.Response.Headers.Add("WWW-Authenticate",
              "Basic Scheme='CountingKs' location='http://localhost:57580/account/login'");

        }
    }
}