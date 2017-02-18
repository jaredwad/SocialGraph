namespace Logic.Twitter
{
    public struct TwitterAuth
    {
        public TwitterAuth(string consimerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            ConsumerKey = consimerKey;
            ConsumerSecret = consumerSecret;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}