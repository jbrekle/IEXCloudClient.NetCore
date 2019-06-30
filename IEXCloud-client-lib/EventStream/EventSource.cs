using System;
using EvtSource;

namespace IEXCloudClient.NetCore.EventStream
{
    public interface IEventSource<T>
    {
        event EventHandler<T> Events;
    }

    public abstract class EventSource<T> : IEventSource<T>
    {
		private int _eventSubscribers = 0;
		protected EventHandler<T> _eventHandler;
        protected EventSourceReader _eventSource;
        private readonly object _eventLock = new object();
        
        public event EventHandler<T> Events
		{
			add 
			{
				lock(_eventLock){
					_eventHandler += value;
					_eventSubscribers++;
					if(_eventSubscribers == 1){
						StartListener();
					}
				}
			}
			remove
			{
				lock(_eventLock){
					_eventHandler -= value;
					_eventSubscribers--;
					if(_eventSubscribers == 0){
						StopListener();
					}
				}
			}
		}

        protected abstract void StartListener();
        protected abstract void StopListener();
    }
}