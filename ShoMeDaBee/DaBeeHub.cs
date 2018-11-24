using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShoMeDaBee.Internal;

namespace ShoMeDaBee
{
    public class DaBeeHub : Hub
    {
        public virtual DaBeeSession Session => DaBeeSession.Current;

        public virtual Task StartSession(string contextName)
            => Session.StartSession(this, contextName);

        public virtual Task ChangeState(
            DaBeeEntityEntry entityEntry,
            EntityState oldState,
            bool fromQuery)
            => Session.ChangeState(this, entityEntry, oldState, fromQuery);

        public virtual Task SaveStarting()
            => Session.SaveStarting(this);

        public virtual Task SaveCompleted(int saved)
            => Session.SaveCompleted(this, saved);
    }
}