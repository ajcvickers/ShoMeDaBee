using Microsoft.EntityFrameworkCore;

namespace ShoMeDaBee
{
    public interface IDaBeeSession
    {
        void StartSession(string contextName);

        void ChangeState(
            string entityType,
            EntityState oldState,
            EntityState newState,
            bool fromQuery);

        void SaveStarting();

        void SaveCompleted(int saved);
    }
}