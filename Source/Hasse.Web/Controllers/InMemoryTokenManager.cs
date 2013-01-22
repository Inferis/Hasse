using System;
using System.Collections.Generic;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;

namespace Hasse.Web.Controllers
{
    internal class InMemoryTokenManager : IConsumerTokenManager
    {
        private readonly Dictionary<string, string> tokensAndSecrets = new Dictionary<string, string>();

        public InMemoryTokenManager(string consumerKey, string consumerSecret)
        {
            if (string.IsNullOrEmpty(consumerKey)) {
                throw new ArgumentNullException("consumerKey");
            }

            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
        }

        public string ConsumerKey { get; private set; }

        public string ConsumerSecret { get; private set; }

        public string GetTokenSecret(string token)
        {
            return this.tokensAndSecrets[token];
        }

        public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response)
        {
            this.tokensAndSecrets[response.Token] = response.TokenSecret;
        }

        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
        {
            this.tokensAndSecrets.Remove(requestToken);
            this.tokensAndSecrets[accessToken] = accessTokenSecret;
        }

        public TokenType GetTokenType(string token)
        {
            throw new NotImplementedException();
        }

    }
}