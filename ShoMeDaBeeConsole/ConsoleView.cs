using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ShoMeDaBee
{
    public class ConsoleViewer : IDaBeeViewer
    {
        private const int EventsTop = 1;
        private const int EventsLines = 5;
        private const int TrackingTop = EventsTop + EventsLines;
        private const int TrackingNameWidth = 20;

        private readonly EventScroller _eventScroller 
            = new EventScroller(top: EventsTop, lines: EventsLines);

        public void Reset(string contextName)
        {
            Console.CursorVisible = false;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.DarkBlue;

            Console.Write($"Events from: {contextName}".PadRight(Console.BufferWidth));

            Console.SetCursorPosition(0, TrackingTop);
            Console.Write("Currently tracking: (U=Unchanged; D=Deleted; M=Modified; A=Added)".PadRight(Console.BufferWidth));
        }

        public void AddEvent(string message)
        {
            _eventScroller.Add(message);
        }

        public void PatchEvent(string message)
        {
            _eventScroller.Patch(message);
        }

        public void UpdateTracking(IEnumerable<(string EntityType, int[] StateCounts)> tracked)
        {
            Console.BackgroundColor = ConsoleColor.Black;

            var row = TrackingTop + 1;
            foreach (var entry in tracked.OrderBy(e => e.EntityType))
            {
                var entityType = entry.EntityType;
                if (entityType.Length > TrackingNameWidth)
                {
                    entityType = entityType.Substring(0, TrackingNameWidth);
                }

                var message = $" {entityType}: ".PadRight(TrackingNameWidth + 3);

                for (var i = 1; i < 5; i++)
                {
                    if (entry.StateCounts[i] > 0)
                    {
                        message += $"{((EntityState)i).ToString()[0]}{entry.StateCounts[i]} ";
                    }
                }

                message = message.PadRight(Console.BufferWidth);

                Console.SetCursorPosition(0, row++);
                Console.Write(message);
            }
        }
    }
}