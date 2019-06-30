using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IEXCloudClient.NetCore.EventStream;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace IEXCloudClient.NetCore
{
    public class IEXCloudClient : IIEXCloudClient
	{
        public readonly IEXCloudClientOptions Options;

        private string _host { get; }

        private const string SCHEME = "https://";

		private const string IEXDOMAIN = "iexapis.com";

		internal readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly HttpClient _httpClient;
        private readonly string _url;

		private static readonly Throtttler _throttler = new Throtttler();

        public long MessagesUsedTotal { get; internal set; }

        public IEXCloudClient(IEXCloudClientOptions options, IHttpClientFactory httpClientFactory)
		{
            Options = options;
			_host = Options.Environment + "." + IEXDOMAIN ;
			_url = SCHEME + _host + "/" + Options.Version;
			_jsonSerializerSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
			};
			_httpClient = httpClientFactory.CreateClient();
			_httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, this.GetType().FullName);
        }

        private void Sign(HttpRequestMessage request){
			string access_key = this.Options.PublicToken; // public key
            string secret_key = this.Options.SecretToken; // secret key for public key
			
			var canonical_querystring = request.RequestUri.Query.Substring(1);
			var canonical_uri = request.RequestUri.AbsolutePath;

			var data = GenerateSigningData(request.Method.Method, canonical_uri, canonical_querystring, access_key, secret_key, this._host);

			request.Headers.Add("x-iex-date", data.iexdate);
			request.Headers.TryAddWithoutValidation("Authorization", data.authorization_header);
			request.Headers.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
			//Console.WriteLine(request.Headers.GetValues("Authorization").First());
			//Console.WriteLine(request.Headers.GetValues("x-iex-date").First());
		}

		internal struct SigningData {
			public string iexdate;
			public string authorization_header;
		}

		private static SigningData GenerateSigningData(string request_method, string canonical_uri, string canonical_querystring, string access_key, string secret_key, string host){
			var utcNow = DateTime.UtcNow;
            var iexdate = utcNow.ToString("yyyyMMddTHHmmss") + "Z";
			var datestamp = utcNow.ToString("yyyyMMdd");
			//Console.WriteLine(iexdate);
			//Console.WriteLine(datestamp);
			var canonical_headers = "host:" + host + "\n" + "x-iex-date:" + iexdate + "\n";
			var signed_headers = "host;x-iex-date";
			var payload = "";
			var payload_hash = CreateHash(payload);
			var canonical_request = request_method + "\n" + canonical_uri + "\n" + canonical_querystring + "\n" + canonical_headers + "\n" + signed_headers + "\n" + payload_hash;
			//Console.WriteLine(canonical_request);

			var algorithm = "IEX-HMAC-SHA256";
			var credential_scope = datestamp + "/" + "iex_request";
			var string_to_sign = algorithm + "\n" +  iexdate + "\n" +  credential_scope + "\n" + CreateHash(canonical_request);
            var signing_key = CreateSignatureKey(secret_key, datestamp);
			//Console.WriteLine(string_to_sign);
			//Console.WriteLine(signing_key);
			var signature = CreateHmac(signing_key, string_to_sign);

			//Console.WriteLine(signature);
            var authorization_header = algorithm + " " + "Credential=" + access_key + "/" + credential_scope + ", " +  "SignedHeaders=" + signed_headers + ", " + "Signature=" + signature;
			//Console.WriteLine(authorization_header);

			return new SigningData() {
				authorization_header = authorization_header,
				iexdate = iexdate
			};
		}
		
		private static string CreateHash(string input)
		{
			var myEncoder = new System.Text.UTF8Encoding();
			var Text = myEncoder.GetBytes(input);
			var myHMACSHA1 = new System.Security.Cryptography.SHA256Managed();
			var HashCode = myHMACSHA1.ComputeHash(Text);
			var hash =  BitConverter.ToString(HashCode).Replace("-", "");
			return hash.ToLower();
		}
		
		private static string CreateHmac(string secret, string data)
		{
			var myEncoder = new System.Text.UTF8Encoding();
			var secretBytes = myEncoder.GetBytes(secret);
			var dataBytes = myEncoder.GetBytes(data);
			var myHMACSHA1 = new System.Security.Cryptography.HMACSHA256(secretBytes);
			var HashCode = myHMACSHA1.ComputeHash(dataBytes);
			var hash =  BitConverter.ToString(HashCode).Replace("-", "");
			return hash.ToLower();
		}

		private static string Sign(string secret, string data) {
			return CreateHmac(secret, data);
		}

		private static string CreateSignatureKey(string key, string datestamp) {
			var signedDate = Sign(key, datestamp);
			return Sign(signedDate, "iex_request");
		}

        private async Task<string> GetResponseText(string url, bool sign = false)
        {
			var request = new HttpRequestMessage() { 
				RequestUri = new Uri(url) 
			};
			if(sign) Sign(request);
            await _throttler.Throttle();
            var response = await _httpClient.SendAsync(request);
            AssertSuccess(response);
            var text = await response.Content.ReadAsStringAsync();
            return text;
        }

		private async Task<TInterface> GetRemoteObject<TInterface, TImpl>(string url) 
			 where TImpl : TInterface
		{
			var text = await GetResponseText(url);
            return JsonConvert.DeserializeObject<TImpl>(text, _jsonSerializerSettings);
		}

		private async Task<T> GetRemoteObject<T>(string url) 
		{
			var text = await GetResponseText(url);
            return JsonConvert.DeserializeObject<T>(text, _jsonSerializerSettings);
		}

		private async Task<List<TInterface>> GetRemoteObjects<TInterface, TImplementation>(string url){
			var objects = await GetRemoteObject<List<TImplementation>, List<TImplementation>>(url);
            return objects.Cast<TInterface>().ToList();
		}

        private void AssertSuccess(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(response.StatusCode.ToString());
            }
			TrackMessageCount(response);
        }

        private void TrackMessageCount(HttpResponseMessage response)
        {
            if (response.Headers.TryGetValues("iexcloud-messages-used", out IEnumerable<string> values))
				if(Int64.TryParse(values.First(), out long value))
                	MessagesUsedTotal += value;
        }

		internal class JsonContent : StringContent
		{
			public JsonContent(object obj) :
				base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
			{ }
		}

		public Task<IQuote> GetQuote(string symbol)
        {
            return GetRemoteObject<IQuote, Quote>($"{_url}/stock/{symbol}/quote?token={Options.PublicToken}");
        }
		
		public async Task<SignedTokenResponse> GetSignedToken()
        {
			if(signedTokenResponse != null){
				return signedTokenResponse; //caching
			}

            var request = new HttpRequestMessage() { 
				Method = HttpMethod.Post,
				RequestUri = new Uri($"{_url}/account/signed"),
				Content = new JsonContent(new {
					token = Options.SecretToken, 
					tokenToSign = Options.PublicToken
				})
			};
			request.Headers.Accept.ParseAdd("application/json");
            var response = await _httpClient.SendAsync(request);
            	
            var text = await response.Content.ReadAsStringAsync();
			try {
                signedTokenResponse = JsonConvert.DeserializeObject<SignedTokenResponse>(text, _jsonSerializerSettings);
                return signedTokenResponse;
			} catch(JsonReaderException) {
				throw new Exception(text);
			}
        }

		public async Task<IAccountMetadata> GetAccountMetadata()
        {
			var skToken = await GetSignedToken();
            var account = await GetRemoteObject<IAccountMetadata, AccountMetadata>($"{_url}/account/metadata?token={skToken.SignedToken}");
			return account;
		}
		
		public Task<IApiStatus> GetApiStatus()
        {
            return GetRemoteObject<IApiStatus, ApiStatus>($"{_url}/status");
        }
		
		public Task<ISystemEvent> GetSystemEvents()
        {
			// not working yet, returns empty object
			// documentation says nothing about needed token but without token, throws BadRequest
			return GetRemoteObject<ISystemEvent, SystemEvent>($"{_url}/deep/system-event?token={Options.PublicToken}");
        }

        public Task<List<IQuote>> GetList(MarketListCriteria property, int limit = 0)
		{
			var limitStr = limit > 0 ? $"&listLimit={limit}" : string.Empty;
            string propLower = property.ToString().ToLowerInvariant();
            return GetRemoteObjects<IQuote, Quote>($"{_url}/stock/market/list/{propLower}?token={Options.PublicToken}{limitStr}");
		}

		public Task<ICompany> GetCompany(string symbol)
        {
            return GetRemoteObject<ICompany, Company>($"{_url}/stock/{symbol}/company?token={Options.PublicToken}");
        }

		public async Task<ICompanyFull> GetCompanyFull(string symbol, DividendRange dividendRange)
		{
			var company = await GetCompany(symbol);
			var fullCompany = new CompanyFull();
			fullCompany.BalanceSheets = await GetBalanceSheets(symbol);
            fullCompany.Dividends = await GetDividends(symbol, dividendRange);
            fullCompany.Quote = await GetQuote(symbol);
            fullCompany.Earnings = await GetEarnings(symbol);
			return fullCompany;
		}

        public Task<ILogo> GetLogo(string symbol)
		{
            return GetRemoteObject<ILogo, Logo>($"{_url}/stock/{symbol}/logo?token={Options.PublicToken}");
		}

		public async Task<List<IBalanceSheet>> GetBalanceSheets(string symbol)
		{			
			var text = await GetResponseText($"{_url}/stock/{symbol}/balance-sheet?token={Options.PublicToken}");
			if (text == "{}")
			{
				return new List<IBalanceSheet>();
			}
			var res = JsonConvert.DeserializeObject<BalanceSheetResponse>(text, _jsonSerializerSettings);
			return res.BalanceSheets.Cast<IBalanceSheet>().ToList();
		}

		public Task<List<IDividend>> GetDividends(string symbol, DividendRange range)
		{
			string rangeStr = "";
			switch(range){

				case DividendRange.YearToDate:
					rangeStr = "ytd";
				break;
				case DividendRange.Next:
					rangeStr = "next";
				break;
				case DividendRange.Months1:
					rangeStr = "1m";
				break;
				case DividendRange.Months3:
					rangeStr = "3m";
				break;
				case DividendRange.Months6:
					rangeStr = "6m";
				break;
				case DividendRange.Years1:
					rangeStr = "1y";
				break;
				case DividendRange.Years2:
					rangeStr = "2y";
				break;
				case DividendRange.Years5:
					rangeStr = "5y";
				break;
			}
			
            return GetRemoteObjects<IDividend, Dividend>($"{_url}/stock/{symbol}/dividends/{rangeStr}?token={Options.PublicToken}");
		}

		public Task<List<string>> GetPeers(string symbol)
		{
			return GetRemoteObject<List<string>>($"{_url}/stock/{symbol}/peers?token={Options.PublicToken}");
		}

		public Task<IDelayedQuote> GetDelayedQuote(string symbol)
		{
            return GetRemoteObject<IDelayedQuote, DelayedQuote>($"{_url}/stock/{symbol}/delayed-quote?token={Options.PublicToken}");
		}

		public async Task<List<IEarnings>> GetEarnings(string symbol, int last = 0)
		{
			var lastStr = last > 0 ? "/"+last : "";
			string text = await GetResponseText($"{_url}/stock/{symbol}/earnings{lastStr}?token={Options.PublicToken}");

            if (text == "{}")
			{
				return new List<IEarnings>();
			}

            EarningResponse earningResponse = JsonConvert.DeserializeObject<EarningResponse>(text, _jsonSerializerSettings);
			return earningResponse.Earnings.Cast<IEarnings>().ToList();
		}

		public async Task<string> GetEarningsProperty(string symbol, EarningsProperties properties)
		{
			var propsString = "/"+properties.ToString()[0].ToString().ToLower()+properties.ToString().Substring(1);
			string text = await GetResponseText($"{_url}/stock/{symbol}/earnings{propsString}?token={Options.PublicToken}");
			return text;
		}

		public async Task<decimal> GetPrice(string symbol)
		{
			string text = await GetResponseText($"{_url}/stock/{symbol}/price?token={Options.PublicToken}");
			return decimal.Parse(text);
		}

		public Task<List<IVenueVolume>> GetVolumebyVenue(string symbol)
		{
			return GetRemoteObjects<IVenueVolume,VenueVolume>($"{_url}/stock/{symbol}/volume-by-venue?token={Options.PublicToken}");
		}

		public Task<List<ISectorPerformance>> GetSectorPerformances()
		{
			return GetRemoteObjects<ISectorPerformance, SectorPerformance>($"{_url}/stock/market/sector-performance?token={Options.PublicToken}");
		}

		public Task<IPriceTarget> GetPriceTarget(string symbol)
		{
			return GetRemoteObject<IPriceTarget, PriceTarget>($"{_url}/stock/{symbol}/price-target?token={Options.PublicToken}");
		}

		public Task<IKeyStats> GetKeyStats(string symbol)
		{
			return GetRemoteObject<IKeyStats, KeyStats>($"{_url}/stock/{symbol}/stats?token={Options.PublicToken}");
		}

		public Task<List<INewsArticle>> GetNews(string symbol, int numOfHeadlines)
		{
			return GetRemoteObjects<INewsArticle, NewsArticle>($"{_url}/stock/{symbol}/news/last/{numOfHeadlines}?token={Options.PublicToken}");
		}

		public Task<IPreviousDayPrice> GetPreviousDayPrice(string symbol)
		{
            return GetRemoteObject<IPreviousDayPrice, PreviousDayPrice>($"{_url}/stock/{symbol}/previous?token={Options.PublicToken}");
		}

		public Task<List<IHistoricalPrice>> GetHistoricalPrices(string symbol, HistoricalPricesRange range, DateTime? specificDate = null)
		{
			string rangeStr = "";
			switch(range){
				case HistoricalPricesRange.Max:
					rangeStr = "max";
				break;
				case HistoricalPricesRange.OneYears:
					rangeStr = "1y";
				break;
				case HistoricalPricesRange.TwoYears:
					rangeStr = "2y";
				break;
				case HistoricalPricesRange.Years5:
					rangeStr = "5y";
				break;
				case HistoricalPricesRange.YearToDate:
					rangeStr = "ytd";
				break;
				case HistoricalPricesRange.OneMonth:
					rangeStr = "1m";
				break;
				case HistoricalPricesRange.ThreeMonth:
					rangeStr = "3m";
				break;
				case HistoricalPricesRange.SixMonth:
					rangeStr = "6m";
				break;
				case HistoricalPricesRange.OneMonth30MinIntervals:
					rangeStr = "1mm";
				break;
				case HistoricalPricesRange.FiveDays:
					rangeStr = "5d";
				break;
				case HistoricalPricesRange.FiveDays10MinIntervals:
					rangeStr = "5dm";
				break;
				case HistoricalPricesRange.Dynamic:
					rangeStr = "dynamic";
				break;
			}

			if(specificDate.HasValue){
				rangeStr = "date/"+specificDate.Value.ToString("yyyyMMdd");
			}
			return GetRemoteObjects<IHistoricalPrice, HistoricalPrice>($"{_url}/stock/{symbol}/chart/{rangeStr}?token={Options.PublicToken}");
		}

		public Task<IOHLC> GetOHLC(string symbol)
		{
			return GetRemoteObject<IOHLC, OHLC>($"{_url}/stock/{symbol}/ohlc?token={Options.PublicToken}");
		}

		public Task<List<IMarketVolumeUS>> GetMarketVolumeUs()
		{
			return GetRemoteObjects<IMarketVolumeUS, MarketVolumeUS>($"{_url}/stock/market/volume?token={Options.PublicToken}");
		}

		private Dictionary<string, TradeEventSource> tradeEventsLookup = new Dictionary<string, TradeEventSource>();
        private SignedTokenResponse signedTokenResponse;

        public IEventSource<ITradeEvent> CreateTradeEventSource(params string[] symbols){
            string key = string.Join(",", symbols);
			if(symbols.Length > 10){
				throw new ArgumentException("maximum of 10 symbols exceeded. got: "+key);
			}
            if (tradeEventsLookup.TryGetValue(key, out TradeEventSource eventhandler)){
				return eventhandler;
			} else {
				var neweventhandler = new TradeEventSource(this, symbols);
				tradeEventsLookup[key] = neweventhandler;
				return neweventhandler;
			}
		}

        public Task<List<ISymbol>> GetSymbols()
        {
			return GetRemoteObjects<ISymbol, SymbolImpl>($"{_url}/ref-data/symbols?token={Options.PublicToken}");
        }
    }
}
