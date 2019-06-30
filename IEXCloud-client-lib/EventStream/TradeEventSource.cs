using System;
using System.Linq;
using System.Threading.Tasks;
using EvtSource;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore.EventStream
{
    public class TradeEventSource : EventSource<ITradeEvent>
    {
        private readonly IEXCloudClient client;
        private readonly string[] symbols;
        private bool started = false;

        public TradeEventSource(IEXCloudClient client, string[] symbols)
        {
            this.client = client;
            this.symbols = symbols;
        }

        protected override void StopListener()
        {
            if(!started) return;
            _eventSource.MessageReceived -= HandleTradeEvent;
			_eventSource.Dispose();
			_eventSource = null;
        }

        protected override void StartListener()
        {
            object symbolsStr = string.Join(",", symbols);
            _eventSource = new EventSourceReader(new Uri($"https://cloud-sse.iexapis.com/{client.Options.Version}/deep?token={client.Options.PublicToken}&symbols={symbolsStr}&channels=trades"));
            _eventSource.MessageReceived += HandleTradeEvent;
            _eventSource.Disconnected += async (object sender, DisconnectEventArgs e) => {
                Console.WriteLine($"Retry after {e.ReconnectDelay}ms - Error: {e.Exception.Message}");
                await Task.Delay(e.ReconnectDelay);
                _eventSource.Start(); // Reconnect to the same URL
            };
            _eventSource.Start();
            started = true;
        }

        private void HandleTradeEvent(object sender, EventSourceMessageEventArgs e) 
        {
            try {
                var tradeMessages = JsonConvert.DeserializeObject<TradeMessage[]>(e.Message, client._jsonSerializerSettings);
                
                var handler = _eventHandler;

                if(tradeMessages == null || !tradeMessages.Any() || handler == null) return;

                var firstMessage = tradeMessages.First();
                var trade = firstMessage.Trade;
                
                trade.Symbol = firstMessage.Symbol;
                
                handler(this, trade);
                //client.MessagesUsedTotal++; not needed - free operation, just for example
            } catch (Exception ex){
                Console.WriteLine(ex.ToString());
            }
        }
    }
}