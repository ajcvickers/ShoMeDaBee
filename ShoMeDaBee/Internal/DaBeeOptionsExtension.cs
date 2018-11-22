using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ShoMeDaBee.Internal
{
    public class DaBeeOptionsExtension : IDbContextOptionsExtension
    {
        private DaBeeDiagnosticListener _listener;
        private TimeSpan? _delay;

        private string _logFragment;

        public DaBeeOptionsExtension()
        {
        }

        protected DaBeeOptionsExtension(DaBeeOptionsExtension copyFrom)
        {
            _listener = copyFrom._listener;
            _delay = copyFrom._delay;
        }

        public virtual string HubUrl => _listener.HubUrl;

        public virtual DaBeeOptionsExtension WithHub(string url, TimeSpan? delay)
        {
            var clone = Clone();

            clone._delay = delay;
            clone._listener = new DaBeeDiagnosticListener(url, delay);
            DiagnosticListener.AllListeners.Subscribe(clone._listener);

            return clone;
        }

        protected virtual DaBeeOptionsExtension Clone() => new DaBeeOptionsExtension(this);

        public bool ApplyServices(IServiceCollection services) => false;

        public long GetServiceProviderHashCode() => 0;

        public void Validate(IDbContextOptions options)
        {
        }

        public virtual string LogFragment
            => _logFragment
               ?? (_logFragment = $"using {_listener.HubUrl}");
    }
}