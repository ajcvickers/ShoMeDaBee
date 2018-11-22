using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ShoMeDaBee
{
    public abstract class DaBeeHub : Hub, IDaBeeSession
    {
        public abstract IDaBeeSession Session { get; }

        public virtual void StartSession(string contextName)
            => Session.StartSession(contextName);

        public virtual void ChangeState(
            string entityType,
            EntityState oldState,
            EntityState newState,
            bool fromQuery)
            => Session.ChangeState(entityType, oldState, newState, fromQuery);

        public virtual void SaveStarting()
            => Session.SaveStarting();

        public virtual void SaveCompleted(int saved)
            => Session.SaveCompleted(saved);
    }
}