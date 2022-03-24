using ALMS.Model;

namespace ALMS.Service
{
    public interface IContextUserIdentity
    {
        void SetContextUser(ContextUser user);
        ContextUser GetContextUser(bool throwError = true);
        void SetToken(string token);
        string GetToken();
    }
}