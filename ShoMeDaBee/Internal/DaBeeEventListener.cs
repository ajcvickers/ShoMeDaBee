using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;

namespace ShoMeDaBee.Internal
{
    public class DaBeeEventListener : ObserverBase<KeyValuePair<string, object>>
    {
        private readonly TimeSpan? _delay;
        private readonly HubConnection _connection;
        private readonly IDictionary<object, Guid> _trackingGuids = new Dictionary<object, Guid>();

        public DaBeeEventListener(string hubUrl, TimeSpan? delay)
        {
            _delay = delay;
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _connection.StartAsync();
        }

        public override void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Value is EventData eventData)
            {
                if (eventData.EventId.Id == CoreEventId.ContextInitialized.Id)
                {
                    var context = ((ContextInitializedEventData)eventData).Context;
                    _connection.InvokeAsync(nameof(DaBeeHub.StartSession), context.GetType().ShortDisplayName());
                    
                    context.ChangeTracker.Tracked += Tracked;
                    context.ChangeTracker.StateChanged += StateChanged;
                }
                else if (eventData.EventId.Id == CoreEventId.SaveChangesStarting)
                {
                    _connection.InvokeAsync(nameof(DaBeeHub.SaveStarting));
                }
                else if (eventData.EventId.Id == CoreEventId.SaveChangesCompleted)
                {
                    _connection.InvokeAsync(nameof(DaBeeHub.SaveCompleted), ((SaveChangesCompletedEventData)eventData).EntitiesSavedCount);
                }
            }
        }

        private void Tracked(object sender, EntityTrackedEventArgs args)
        {
            var trackingGuid = Guid.NewGuid();
            _trackingGuids[args.Entry.Entity] = trackingGuid;

            _connection.InvokeAsync(
                nameof(DaBeeHub.ChangeState),
                new DaBeeEntityEntry(args.Entry, trackingGuid),
                EntityState.Detached,
                args.FromQuery);

            IntroduceDelay();
        }

        private void IntroduceDelay()
        {
            if (_delay.HasValue)
            {
                Thread.Sleep((int) _delay.Value.TotalMilliseconds);
            }
        }

        private void StateChanged(object sender, EntityStateChangedEventArgs args)
        {
            _connection.InvokeAsync(
                nameof(DaBeeHub.ChangeState),
                new DaBeeEntityEntry(args.Entry, _trackingGuids[args.Entry.Entity]),
                args.OldState,
                false);

            IntroduceDelay();
        }
    }
}