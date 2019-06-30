using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IEXCloudClient.NetCore.Helper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace IEXCloudClient.NetCore.Tests
{
    [TestFixture]
    public class IEXCloudClientIntegrationTests
    {
        private IIEXCloudClient client;
        private const string Symbol = "aapl";

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddIEXClientWithAppSettingsConfig();
            var serviceProvider = services.BuildServiceProvider();
            client = serviceProvider.GetRequiredService<IIEXCloudClient>();
        }

        [Test]
        [Explicit]
        public async Task TestGetAccountMetadata()
        {
            var result = await client.GetAccountMetadata();
            Assert.That(result.MessageLimit, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetApiStatus()
        {
            var result = await client.GetApiStatus();
            Assert.That(result.StatusUp);
        }

        [Test]
        [Explicit]
        public async Task TestGetBalanceSheets()
        {
            var result = await client.GetBalanceSheets(Symbol);
            Assert.That(result.First().TotalAssets, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetCompany()
        {
            var result = await client.GetCompany(Symbol);
            Assert.That(result.CompanyName, Is.Not.Null);
        }

        [Test]
        [Explicit]
        public async Task TestGetDelayedQuote()
        {
            var result = await client.GetDelayedQuote(Symbol);
            Assert.That(result.DelayedPrice, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetDividends()
        {
            var result = await client.GetDividends(Symbol, DividendRange.Years1);
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result.First().Amount, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetEarnings()
        {
            var result = await client.GetEarnings(Symbol, 1);
            Assert.That(result.First().EPSReportDate, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        [Explicit]
        public async Task TestGetEarningsProperty()
        {
            var result = await client.GetEarningsProperty(Symbol, EarningsProperties.EPSReportDate);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Explicit]
        public async Task TestGetHistoricalPrices()
        {
            var result = await client.GetHistoricalPrices(Symbol, HistoricalPricesRange.OneMonth);
            Assert.That(result.Count, Is.GreaterThan(1));
            Assert.That(result.First().High, Is.Not.EqualTo(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetKeyStats()
        {
            var result = await client.GetKeyStats(Symbol);
            Assert.That(result.Week52High, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetList()
        {
            var result = await client.GetList(MarketListCriteria.Gainers);
            Assert.That(result.Count, Is.GreaterThan(3));
            Assert.That(result.First().Week52High, Is.GreaterThan(3));
        }

        [Test]
        [Explicit]
        public async Task TestGetLogo()
        {
            var result = await client.GetLogo(Symbol);
            Assert.That(result.URL, Is.Not.Null);
        }

        [Test]
        [Explicit]
        public async Task TestGetMarketVolumeUs()
        {
            var result = await client.GetMarketVolumeUs();
            Assert.That(result.First().MarketPercent, Is.Not.EqualTo(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetNews()
        {
            var result = await client.GetNews(Symbol,1);
            Assert.That(result.First().Summary, Is.Not.Null);
        }

        [Test]
        [Explicit]
        public async Task TestGetOHLC()
        {
            var result = await client.GetOHLC(Symbol);
            Assert.That(result.Open.Price, Is.GreaterThan(0));
            Assert.That(result.Open.Time, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        [Explicit]
        public async Task TestGetPeers()
        {
            var result = await client.GetPeers(Symbol);
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result.First(), Is.Not.Null);
        }

        [Test]
        [Explicit]
        public async Task TestGetPreviousDayPrice()
        {
            var result = await client.GetPreviousDayPrice(Symbol);
            Assert.That(result.Volume, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetPrice()
        {
            var result = await client.GetPrice(Symbol);
            Assert.That(result, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetPriceTarget()
        {
            var result = await client.GetPriceTarget(Symbol);
            Assert.That(result.PriceTargetAverage, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetQuote()
        {
            var result = await client.GetQuote(Symbol);
            Assert.That(result.LatestPrice, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestGetSectorPerformances()
        {
            var result = await client.GetSectorPerformances();
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result.First().Performance, Is.Not.EqualTo(0));
            Assert.That(result.First().Name, Is.Not.Null);
        }

        [Test]
        [Explicit]
        public async Task TestGetSignedToken()
        {
            var result = await client.GetSignedToken();
            Assert.That(result.SignedToken, Is.Not.Null);
        }

        [Test]
        [Explicit]
        public async Task TestGetSymbols()
        {
            var result = await client.GetSymbols();
            Assert.That(result.Count, Is.GreaterThan(100));
        }

        [Test]
        [Explicit]
        public async Task TestGetSystemEvents()
        {
            var result = await client.GetSystemEvents();
            Assert.That(result.Time, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        [Explicit]
        public async Task TestGetVolumebyVenue()
        {
            var result = await client.GetVolumebyVenue(Symbol);
            Assert.That(result.Count, Is.GreaterThan(1));
            Assert.That(result.First().VenueName, Is.Not.Null);
            Assert.That(result.First().Volume, Is.GreaterThan(0));
        }

        [Test]
        [Explicit]
        public async Task TestMessagesUsedTotal()
        {
            await client.GetQuote(Symbol);
            var messagesBefore = client.MessagesUsedTotal;
            await client.GetQuote(Symbol);
            var messagesAfter = client.MessagesUsedTotal;
            Assert.That(messagesAfter, Is.GreaterThan(messagesBefore));
        }

        [Test]
        [Explicit]
        public void TestTradeEvents()
        {
            var eventSource = client.CreateTradeEventSource(Symbol);
            var eventReceived = false;
            eventSource.Events += (sender, eventArg) => {
                Console.WriteLine(eventArg.Price);
                eventReceived = true;
            };
            for (int i = 0; i < 10; i++)
            {
                if(eventReceived) {
                    return; //we don't reach the fail below
                }
                Thread.Sleep(1000);
            }
            Assert.Fail("no trade event received");
        }
    }
}