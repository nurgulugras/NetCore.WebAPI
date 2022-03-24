namespace ALMS.Model
{
    public class ContextUser
    {
        public ContextUser(User user, HttpRequestUserInfo requestUserInfo)
        {
            User = user;
            LoginType = LoginType.UI;
            RequestUserInfo = requestUserInfo;
        }
        public ContextUser(App app, HttpRequestUserInfo requestUserInfo)
        {
            App = app;
            LoginType = LoginType.API;
            RequestUserInfo = requestUserInfo;
        }
        public App App { get; set; }
        public User User { get; set; }
        public LoginType LoginType { get; set; }
        public HttpRequestUserInfo RequestUserInfo { get; set; }
    }
}