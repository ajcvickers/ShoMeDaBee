using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ShoMeDaBee.Internal
{
    public class DaBeeDiagnosticListener : ObserverBase<DiagnosticListener>
    {
        public DaBeeDiagnosticListener(string hubUrl, TimeSpan? delay)
        {
            HubUrl = hubUrl;
            Delay = delay;
        }

        public string HubUrl { get; }
        public TimeSpan? Delay { get; }

        public override void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == DbLoggerCategory.Name)
            {
                listener.Subscribe(new DaBeeEventListener(HubUrl, Delay));
            }
        }
    }
}