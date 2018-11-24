using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ShoMeDaBee.Internal;

namespace ShoMeDaBee
{
    public static class DaBeeClientProxy
    {
        public static Task Reset(this DaBeeHub hub, string contextName) 
            => hub.Clients.All.SendAsync(nameof(Reset), contextName);

        public static Task AddEvent(this DaBeeHub hub, string message)
            => hub.Clients.All.SendAsync(nameof(AddEvent), message);

        public static Task PatchEvent(this DaBeeHub hub, string message)
            => hub.Clients.All.SendAsync(nameof(PatchEvent), message);

        public static Task Track(this DaBeeHub hub, DaBeeEntityEntry entry)
            => hub.Clients.All.SendAsync(nameof(Track), entry);

        public static Task Untrack(this DaBeeHub hub, DaBeeEntityEntry entry)
            => hub.Clients.All.SendAsync(nameof(Untrack), entry);

        public static Task Update(this DaBeeHub hub, DaBeeEntityEntry entry)
            => hub.Clients.All.SendAsync(nameof(Update), entry);
    }
}