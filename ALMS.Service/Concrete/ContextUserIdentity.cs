using ALMS.Core;
using ALMS.Model;

namespace ALMS.Service
{
    public class ContextUserIdentity : IContextUserIdentity
    {
        private ContextUser _contextUser;
        private string _token;

        public void SetToken(string token)
        {
            _token = token;
        }
        public void SetContextUser(ContextUser user)
        {
            _contextUser = user;
        }
        public ContextUser GetContextUser(bool throwError = true)
        {
            if (_contextUser == null && throwError)
                throw new UnauthorizedException("Kullanıcı sistemde aktif gözükmüyor. Lütfen tekrar oturum açınız.");
            return _contextUser;
        }
        public string GetToken() => _token;
    }
}