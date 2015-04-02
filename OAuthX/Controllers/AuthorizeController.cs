using OAuthX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace oAuthX.Controllers
{
    public class AuthorizeController : ApiController
    {
        private dbContext db = new dbContext();

        // GET: api/Authorize
        [ResponseType(typeof(OAuthTransaction))]
        public async Task<IHttpActionResult> GetAuthorize()
        {
            Uri myUri = Request.RequestUri;
            if (String.IsNullOrWhiteSpace(myUri.ParseQueryString()["code"]))
                return Ok("Problem, no code returned.");
            if (String.IsNullOrWhiteSpace(myUri.ParseQueryString()["state"]))
                return Ok("Problem, no state returned.");
            
            Dictionary<String, String> stateVals = parseStateValue(myUri);
            String authToken = String.Empty;
            String oAuthTransactionId = String.Empty;

            switch (stateVals["authtype"])
            {
                case "LWA":
                    authToken = myUri.ParseQueryString()["code"];
                    oAuthTransactionId = stateVals["id"];
                    break;
                default:
                    return Ok("Unable to parse values");
            }

            var x = db.OAuthTransactions.First<OAuthTransaction>(a => a.id.Value.ToString() == oAuthTransactionId);
            x.authCode = authToken;
            x.authComplete = true;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {/*
                if (OAuthTransactionExists(oAuthTransaction.id))
                {
                    //do nothing, we can still pass the url for the existing one.
                    //TODO: give helpful error, etxc
                }
                else
                {
                    throw;
                }*/
            }
            return Ok("Thanks, you can close this window now - your application will pick it up from here!");
        }

        public Dictionary<String,String> parseStateValue(Uri x)
        {
            Regex stateRegex = new Regex(@"(?<label>oauthx)?(?<authtype>[A-Za-z]+)?(?<id>[A-Fa-f0-9]{8}(?:-[A-Fa-f0-9]{4}){3}-[A-Fa-f0-9]{12})");
            var groups = stateRegex.Match(x.ParseQueryString()["state"]).Groups;
            return (from String m in stateRegex.GetGroupNames()
                   select new
                   {
                       key = m,
                       value = groups[m].Value
                   }
                    ).ToDictionary(p => p.key, p => p.value);
        }

    }
}
