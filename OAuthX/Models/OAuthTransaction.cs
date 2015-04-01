using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthX.Models
{
    public class OAuthTransaction
    {
        public Guid? id { get; set; }
        public String authUrl {get; set;}
        public Boolean authComplete { get; set; }
        public String authCode { get; set; }

        public OAuthTransaction()
        {
            if (!id.HasValue) id = Guid.NewGuid();
            authComplete = false;
            authCode = String.Empty;
        }


        public OAuthTransaction(Guid nId, String nAuthUrl) : base()
        {
            id = nId;
            authUrl = nAuthUrl;
        }

    }


}