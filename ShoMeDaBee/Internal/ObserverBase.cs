using System;

namespace ShoMeDaBee.Internal
{
    public abstract class ObserverBase<T> : IObserver<T>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public virtual void OnNext(T value)
        {
        }
    }
}