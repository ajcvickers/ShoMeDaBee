using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShoMeDaBee.Internal
{
    public class DaBeeSession
    {
        public static DaBeeSession Current { get; } = new DaBeeSession();

        private (string EntityType, EntityState OldState, EntityState NewState, bool FromQuery) _lastStateChange;
        private int _stateChangeCount;
        private bool _inSave;
        
        public virtual Task StartSession(DaBeeHub hub, string contextName) 
            => hub.Reset(contextName);

        public virtual async Task ChangeState(
            DaBeeHub hub,
            DaBeeEntityEntry entityEntry,
            EntityState oldState,
            bool fromQuery)
        {
            var entityType = entityEntry.EntityTypeName;
            var newState = entityEntry.State;

            if (entityType == _lastStateChange.EntityType
                && oldState == _lastStateChange.OldState
                && entityEntry.State == _lastStateChange.NewState
                && fromQuery == _lastStateChange.FromQuery)
            {
                ++_stateChangeCount;

                if (oldState == EntityState.Detached)
                {
                    await hub.PatchEvent(!fromQuery
                        ? $"Attached {_stateChangeCount} '{entityType}' as {newState}"
                        : $"Query tracked {_stateChangeCount} '{entityType}'");
                }
                else
                {
                    await hub.PatchEvent(_inSave
                        ? $"Saved {_stateChangeCount} '{entityType}'"
                        : $"Changed {_stateChangeCount} '{entityType}' from {oldState} to {newState}");
                }
            }
            else
            {
                if (oldState == EntityState.Detached)
                {
                    await hub.AddEvent(!fromQuery
                        ? $"Attached '{entityType}' as {newState}"
                        : $"Query tracked '{entityType}'");
                }
                else
                {
                    await hub.AddEvent(_inSave
                        ? $"Saved '{entityType}'"
                        : $"Changed '{entityType}' from {oldState} to {newState}");
                }

                _lastStateChange = (entityType, oldState, newState, fromQuery);
                _stateChangeCount = 1;
            }

            if (oldState == EntityState.Detached)
            {
                await hub.Track(entityEntry);
            }
            else if (newState == EntityState.Detached)
            {
                await hub.Untrack(entityEntry);
            }
            else
            {
                await hub.Update(entityEntry);
            }
        }

        public Task SaveStarting(DaBeeHub hub)
        {
            _inSave = true;
            
            return hub.AddEvent("Saving changes...");
        }

        public Task SaveCompleted(DaBeeHub hub, int saved)
        {
            _inSave = false;

            return hub.AddEvent($"Completed save of {saved} entities");
        }
    }
}