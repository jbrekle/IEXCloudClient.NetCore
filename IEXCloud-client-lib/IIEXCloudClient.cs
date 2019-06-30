using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IEXCloudClient.NetCore.EventStream;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public interface IIEXCloudClient
    {
        long MessagesUsedTotal { get; }
        Task<IAccountMetadata> GetAccountMetadata();
        Task<IApiStatus> GetApiStatus();
        Task<List<IBalanceSheet>> GetBalanceSheets(string symbol);
        Task<ICompany> GetCompany(string symbol);
        Task<IDelayedQuote> GetDelayedQuote(string symbol);
        Task<List<IDividend>> GetDividends(string symbol, DividendRange range);
        Task<List<IEarnings>> GetEarnings(string symbol, int last = 0);
        Task<string> GetEarningsProperty(string symbol, EarningsProperties properties);
        Task<List<IHistoricalPrice>> GetHistoricalPrices(string symbol, HistoricalPricesRange range, DateTime? specificDate = null);
        Task<IKeyStats> GetKeyStats(string symbol);
        Task<List<IQuote>> GetList(MarketListCriteria property, int limit = 0);
        Task<ILogo> GetLogo(string symbol);
        Task<List<IMarketVolumeUS>> GetMarketVolumeUs();
        Task<List<INewsArticle>> GetNews(string symbol, int numOfHeadlines);
        Task<IOHLC> GetOHLC(string symbol);
        Task<List<string>> GetPeers(string symbol);
        Task<IPreviousDayPrice> GetPreviousDayPrice(string symbol);
        Task<decimal> GetPrice(string symbol);
        Task<IPriceTarget> GetPriceTarget(string symbol);
        Task<IQuote> GetQuote(string symbol);
        Task<List<ISectorPerformance>> GetSectorPerformances();
        ///Only available to Grow and Scale users
        Task<SignedTokenResponse> GetSignedToken();
        Task<ISystemEvent> GetSystemEvents();
        Task<List<IVenueVolume>> GetVolumebyVenue(string symbol);
        IEventSource<ITradeEvent> CreateTradeEventSource(params string[] symbols);
        Task<List<ISymbol>> GetSymbols();
    }
}
