using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitterizer;

namespace turniri.Social.Twitter
{
    /// <summary>
    /// A consumer capable of communicating with Twitter.
    /// </summary>
    public class TwitterProvider
    {
        public ITwitterAppConfig Config { get; set; }

        public TwitterAccessToken AccessToken { get; set; }

        public string Authorize(string redirectTo)
        {
            OAuthTokenResponse requestToken = OAuthUtility.GetRequestToken(Config.twitterConsumerKey, Config.twitterConsumerSecret, redirectTo);

            // Direct or instruct the user to the following address:
            Uri authorizationUri = OAuthUtility.BuildAuthorizationUri(requestToken.Token);

            return authorizationUri.ToString();
        }

        public TwitterAccessToken GetAuthToken(string requestToken, string verifier)
        {
            var accessToken = OAuthUtility.GetAccessToken(Config.twitterConsumerKey, Config.twitterConsumerSecret,
                                                      requestToken, verifier);

            AccessToken = new TwitterAccessToken()
            {
                Token = accessToken.Token,
                TokenSecret = accessToken.TokenSecret,
                UserId = accessToken.UserId
            };
            return AccessToken;
        }

        public string GetUserInfo(decimal userId)
        {
            var tokens = new OAuthTokens();
            tokens.ConsumerKey = Config.twitterConsumerKey;
            tokens.ConsumerSecret = Config.twitterConsumerSecret;
            tokens.AccessToken = AccessToken.Token;
            tokens.AccessTokenSecret = AccessToken.TokenSecret;

            var response = TwitterUser.Show(tokens, userId);
            return response.Content;
        }

        public void Publish(ISocialPost post)
        {
            var tokens = new OAuthTokens();
            tokens.ConsumerKey = Config.twitterConsumerKey;
            tokens.ConsumerSecret = Config.twitterConsumerSecret;
            tokens.AccessToken = AccessToken.Token;
            tokens.AccessTokenSecret = AccessToken.TokenSecret;
            TwitterStatus.Update(tokens, post.Title + " " + post.Link);
            post.Identifier = AccessToken.UserId.ToString();
        }

    }
}
