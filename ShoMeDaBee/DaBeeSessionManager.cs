using System;
using ShoMeDaBee.Internal;

namespace ShoMeDaBee
{
    public static class DaBeeSessionManager
    {
        private static DaBeeSession _current;

        public static DaBeeSession Current
        {
            get
            {
                if (_current == null)
                {
                    throw new InvalidOperationException("Attempt to use session before initialization.");
                }

                return _current;
            }
        }

        public static void Initialize(IDaBeeViewer viewer)
        {
            _current = new DaBeeSession(viewer);
        }
    }
}