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
        public authTypes authType { get; set; }

        public enum authTypes { other, loginWithAmazon, facebook, twitter }


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

        public OAuthTransaction(Guid nId, String nAuthUrl, String nAuthType) : base()
        {
            id = nId;
            authUrl = nAuthUrl;

            authTypes newAuthType = authTypes.other;
            Enum.TryParse(nAuthType, true, out newAuthType);
            authType = newAuthType;
        }

    }


}