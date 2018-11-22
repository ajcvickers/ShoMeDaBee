using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ShoMeDaBee.Internal
{
    public class DaBeeSession : IDaBeeSession
    {
        private readonly IDaBeeViewer _viewer;
        private readonly IDictionary<string, int[]> _tracked = new Dictionary<string, int[]>();

        private (string EntityType, EntityState OldState, EntityState NewState, bool FromQuery) _lastStateChange;
        private int _stateChangeCount;
        private bool _inSave;

        public DaBeeSession(IDaBeeViewer viewer)
        {
            _viewer = viewer;
        }

        public virtual void StartSession(string contextName)
        {
            _viewer.Reset(contextName);
        }

        public virtual void ChangeState(
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
                    _viewer.PatchEvent(!fromQuery
                        ? $"Attached {_stateChangeCount} '{entityType}' as {newState}"
                        : $"Query tracked {_stateChangeCount} '{entityType}'");
                }
                else
                {
                    _viewer.PatchEvent(_inSave
                        ? $"Saved {_stateChangeCount} '{entityType}'"
                        : $"Changed {_stateChangeCount} '{entityType}' from {oldState} to {newState}");
                }
            }
            else
            {
                if (oldState == EntityState.Detached)
                {
                    _viewer.AddEvent(!fromQuery
                        ? $"Attached '{entityType}' as {newState}"
                        : $"Query tracked '{entityType}'");
                }
                else
                {
                    _viewer.AddEvent(_inSave
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

            _viewer.UpdateTracking(_tracked.Select(e => (e.Key, e.Value)));
        }

        public void SaveStarting()
        {
            _inSave = true;
            
            _viewer.AddEvent("Saving changes...");
        }

        public void SaveCompleted(int saved)
        {
            _inSave = false;

            _viewer.AddEvent($"Completed save of {saved} entities");
        }
    }
}