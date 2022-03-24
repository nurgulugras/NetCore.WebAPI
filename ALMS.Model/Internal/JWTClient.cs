namespace ALMS.Model
{
    public class JWTClient
    {
        public JWTClient() { }
        public JWTClient(JWTUser user)
        {
            UserClient = user;
            LoginType = LoginType.UI;
        }
        public JWTClient(JWTApp app)
        {
            AppClient = app;
            LoginType = LoginType.API;
        }
        public JWTUser UserClient { get; set; }
        public JWTApp AppClient { get; set; }
        public LoginType LoginType { get; set; }
        public HttpRequestUserInfo RequestUserInfo { get; set; }
    }
}