using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;

namespace ShoMeDaBee.Internal
{
    public class ClientDaBeeSession : IDaBeeSession
    {
        private readonly HubConnection _connection;

        public ClientDaBeeSession(string hubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();
        }

        public void Start()
            => _connection.StartAsync();

        public void StartSession(string contextName)
            => _connection.InvokeAsync(nameof(StartSession), contextName);

        public void ChangeState(string entityType, EntityState oldState, EntityState newState, bool fromQuery)
            => _connection.InvokeAsync(nameof(ChangeState), entityType, oldState, newState, fromQuery);

        public void SaveStarting()
            => _connection.InvokeAsync(nameof(SaveStarting));

        public void SaveCompleted(int saved)
            => _connection.InvokeAsync(nameof(SaveCompleted), saved);
    }
}