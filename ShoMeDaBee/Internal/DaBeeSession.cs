using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShoMeDaBee.Internal
{
    public class DaBeeSession
    {
        public static DaBeeSession Current { get; } = new DaBeeSession();

        private readonly DaBeeClientProxy _client;
        private readonly IDictionary<string, int[]> _tracked = new Dictionary<string, int[]>();

        private (string EntityType, EntityState OldState, EntityState NewState, bool FromQuery) _lastStateChange;
        private int _stateChangeCount;
        private bool _inSave;

        public DaBeeSession()
        {
            _client = new DaBeeClientProxy();
        }

        public virtual Task StartSession(DaBeeHub hub, string contextName) 
            => _client.Reset(hub, contextName);

        public virtual async Task ChangeState(
            DaBeeHub hub,
            string entityType,
            EntityState oldState,
            EntityState newState,
            bool fromQuery)
        {
            if (entityType == _lastStateChange.EntityType
                && oldState == _lastStateChange.OldState
                && newState == _lastStateChange.NewState
                && fromQuery == _lastStateChange.FromQuery)
            {
                ++_stateChangeCount;

                if (oldState == EntityState.Detached)
                {
                    await _client.PatchEvent(hub, !fromQuery
                        ? $"Attached {_stateChangeCount} '{entityType}' as {newState}"
                        : $"Query tracked {_stateChangeCount} '{entityType}'");
                }
                else
                {
                    await _client.PatchEvent(hub, _inSave
                        ? $"Saved {_stateChangeCount} '{entityType}'"
                        : $"Changed {_stateChangeCount} '{entityType}' from {oldState} to {newState}");
                }
            }
            else
            {
                if (oldState == EntityState.Detached)
                {
                    await _client.AddEvent(hub, !fromQuery
                        ? $"Attached '{entityType}' as {newState}"
                        : $"Query tracked '{entityType}'");
                }
                else
                {
                    await _client.AddEvent(hub, _inSave
                        ? $"Saved '{entityType}'"
                        : $"Changed '{entityType}' from {oldState} to {newState}");
                }

                _lastStateChange = (entityType, oldState, newState, fromQuery);
                _stateChangeCount = 1;
            }

            if (!_tracked.TryGetValue(entityType, out var counts))
            {
                counts = new int[5];
            }

            if (oldState != EntityState.Detached)
            {
                --counts[(int)oldState];
            }

            if (newState != EntityState.Detached)
            {
                ++counts[(int)newState];
            }

            _tracked[entityType] = counts;

            await _client.UpdateTracking(hub, _tracked.Select(e => (e.Key, e.Value)));
        }

        public Task SaveStarting(DaBeeHub hub)
        {
            _inSave = true;
            
            return _client.AddEvent(hub, "Saving changes...");
        }

        public Task SaveCompleted(DaBeeHub hub, int saved)
        {
            _inSave = false;

            return _client.AddEvent(hub, $"Completed save of {saved} entities");
        }
    }
}