using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ShoMeDaBee.Internal
{
    public class DaBeeEntityEntry
    {
        public DaBeeEntityEntry()
        {
        }

        public DaBeeEntityEntry(EntityEntry entry, Guid trackingGuid)
        {
            EntityTypeName = entry.Metadata.ShortName();
            TrackingGuid = trackingGuid.ToString();
            State = entry.State;
            StateString = entry.State.ToString();

            var keyEntries 
                = entry.Metadata
                    .FindPrimaryKey()
                    .Properties
                    .Select(p => new { p.Name, Entry = entry.Property(p.Name) }).ToList();

            var keyString = "{" + string.Join(", ", keyEntries.Select(p => p.Name + ": " + p.Entry.CurrentValue)) + "}";

            if (keyEntries.Any(e => e.Entry.IsTemporary))
            {
                keyString += " (temp)";
            }

            KeyString = keyString;
            KeyValue = keyEntries.Select(e => e.Entry.CurrentValue).ToArray();

            InfoString = $"Key: {KeyString} State: {State}";
        }

        public string EntityTypeName { get; set; }
        public string TrackingGuid { get; set; }
        public string KeyString { get; set; }
        public object[] KeyValue { get; set; }
        public EntityState State { get; set; }
        public string StateString { get; set; }
        public string InfoString { get; set; }
    }
}