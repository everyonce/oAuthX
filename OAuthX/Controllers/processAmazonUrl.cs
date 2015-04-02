using OAuthX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthX.Controllers
{
    public static class processAmazonUrl
    {
        public static String getLoginString(String originalUriString, OAuthTransaction myTransaction)
        {
            Uri originalUri = new Uri(originalUriString);
            if (String.IsNullOrWhiteSpace(HttpUtility.ParseQueryString(originalUri.Query).Get("state")))
                return originalUri.ToString() + "&state=oauthxLWA" + myTransaction.id;
            else return originalUri.ToString();
        }
    }
}