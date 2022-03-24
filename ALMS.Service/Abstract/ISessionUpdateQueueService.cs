using ALMS.Model;

namespace ALMS.Service
{
    public interface ISessionUpdateQueueService
    {
        void AddQueue(Session session);
        Session GetBeginSessionAndRemove();
        int Count { get; }
        bool HasAny();
    }
}