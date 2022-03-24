using System.Collections.Generic;
using ALMS.Model;

namespace ALMS.Service
{
    public class SessionUpdateQueueService : ISessionUpdateQueueService
    {
        private Queue<Session> _sessionQueue = new Queue<Session>();
        public void AddQueue(Session session)
        {
            var nextSession = session.Shallowcopy();
            nextSession.App = null;
            nextSession.License = null;
            _sessionQueue.Enqueue(nextSession);
        }
        public Session GetBeginSessionAndRemove()
        {
            return _sessionQueue.Dequeue();
        }
        public int Count => _sessionQueue.Count;
        public bool HasAny() => Count > 0;
        public void Dispose() { }
    }
}