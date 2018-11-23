using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ShoMeDaBee
{
    public class DaBeeClientProxy
    {
        public Task Reset(DaBeeHub hub, string contextName) 
            => hub.Clients.All.SendAsync(nameof(Reset), contextName);

        public Task AddEvent(DaBeeHub hub, string message)
            => hub.Clients.All.SendAsync(nameof(AddEvent), message);

        public Task PatchEvent(DaBeeHub hub, string message)
            => hub.Clients.All.SendAsync(nameof(PatchEvent), message);

        public Task UpdateTracking(DaBeeHub hub, IEnumerable<(string EntityType, int[] StateCounts)> tracked)
            => hub.Clients.All.SendAsync(nameof(UpdateTracking), tracked);
    }
}