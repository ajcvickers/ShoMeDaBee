using System.Collections.Generic;

namespace ShoMeDaBee
{
    public interface IDaBeeViewer
    {
        void Reset(string contextName);
        void AddEvent(string message);
        void PatchEvent(string message);
        void UpdateTracking(IEnumerable<(string EntityType, int[] StateCounts)> tracked);
    }
}