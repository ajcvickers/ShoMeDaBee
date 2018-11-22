using System;
using System.Collections.Generic;

namespace ShoMeDaBee
{
    public class EventScroller
    {
        private class Entry
        {
            public string Message { get; set; }
        }

        private readonly int _top;
        private readonly int _lines;

        private readonly Queue<Entry> _entries;
        private Entry _lastEntry;

        public EventScroller(int top, int lines)
        {
            _top = top;
            _lines = lines;
            _entries = new Queue<Entry>(lines);
        }

        private void DisplayEvents()
        {
            Console.BackgroundColor = ConsoleColor.Black;

            var row = _top;
            foreach (var entry in _entries)
            {
                Console.SetCursorPosition(0, row++);
                Console.Write(entry.Message.PadRight(Console.BufferWidth));
            }
        }

        public void Patch(string message)
        {
            _lastEntry.Message = message;

            DisplayEvents();
        }

        public void Add(string message)
        {
            if (_entries.Count >= _lines)
            {
                _entries.Dequeue();
            }

            _lastEntry = new Entry { Message = message };
            _entries.Enqueue(_lastEntry);

            DisplayEvents();
        }
    }
}