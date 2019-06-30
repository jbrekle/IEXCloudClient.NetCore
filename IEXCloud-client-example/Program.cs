using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IEXCloudClient.NetCore.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace IEXCloudClient.NetCore
{
    class Program
    {
        private const string Symbol = "aapl";

        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddIEXClientWithAppSettingsConfig();
            var serviceProvider = services.BuildServiceProvider();
            IIEXCloudClient client = serviceProvider.GetRequiredService<IIEXCloudClient>();

            await TestQuote(client);
            await TestCompany(client);
            //await TestApiStatus(client);
            //await TestAccountMetadata(client);
            //await client.GetSystemEvents();
            //await TestListQuotes(client);
            //await TestHistoricalPrices(client);
            //await TestBalanceSheets(client); //expensive, 3000
            //await TestDividends(client);
            //await TestDelayedQuote(client);
            //await TestEffectiveSpreads(client);
            //await TestOHLC(client);
            //await TestEarnings(client); // expensive 1000
            //await TestMarketVolumeUS(client);
            //await TestLogo(client);
            //await TestPriceTarget(client);
            //await TestNews(client);
            //await TestSectorPerformance(client);
            await TestSymbols(client);
            TestSSE(client);
        }

        private static void TestSSE(IIEXCloudClient client)
        {
            var eventSource = client.CreateTradeEventSource("aapl","SNAP", "fb");
            eventSource.Events += LogTradeEvent;
            Thread.Sleep(100000);
            eventSource.Events -= LogTradeEvent;
        }

        private static void LogTradeEvent(object sender, ITradeEvent e)
        {
             Console.WriteLine("trade event:\n"+e.ToString());
        }

        private async static Task TestMarketVolumeUS(IIEXCloudClient client)
        {
            var mv = await client.GetMarketVolumeUs();
            Console.WriteLine(mv);
        }


        private async static Task TestSymbols(IIEXCloudClient client)
        {
            var symbols = await client.GetSymbols();
            foreach(var symbol in symbols.Take(5)){ //limited to 5
                Console.WriteLine();
                Console.WriteLine(symbol);
            }
        }

        private async static Task TestPriceTarget(IIEXCloudClient client)
        {
            var mv = await client.GetPriceTarget(Symbol);
            Console.WriteLine(mv);
        }

        private async static Task TestEarnings(IIEXCloudClient client)
        {
            var earnings = await client.GetEarnings(Symbol, 2);
            Console.WriteLine(earnings.First());
            var earnings2 = await client.GetEarningsProperty(Symbol, EarningsProperties.ActualEPS);
            Console.WriteLine(earnings2);
        }

        private static async Task TestAccountMetadata(IIEXCloudClient client)
        {
            //var token = await client.GetSignedToken();
            var account = await client.GetAccountMetadata();
        }

        private static async Task TestBalanceSheets(IIEXCloudClient client)
        {
            var balanceSheets = await client.GetBalanceSheets(Symbol);
            Console.WriteLine(balanceSheets.First());
        }

        private static async Task TestDelayedQuote(IIEXCloudClient client)
        {
            var delayedQuote = await client.GetDelayedQuote(Symbol);
            Console.WriteLine(delayedQuote);
        }

        private static async Task TestLogo(IIEXCloudClient client)
        {
            var logo = await client.GetLogo(Symbol);
            Console.WriteLine(logo);
        }

        private static async Task TestDividends(IIEXCloudClient client)
        {
            var dividends = await client.GetDividends(Symbol, DividendRange.Years1);
            Console.WriteLine(dividends.First());
        }

        private static async Task TestListQuotes(IIEXCloudClient client)
        {
            var list = await client.GetList(MarketListCriteria.Gainers);
            foreach(var gainer in list){
                Console.WriteLine();
                Console.WriteLine(gainer.Symbol + " "+gainer.CompanyName);
                Console.WriteLine(gainer.Close + " "+gainer.ChangePercent+"%");
            }
        }
        
        private static async Task TestNews(IIEXCloudClient client)
        {
            var news = await client.GetNews(Symbol, 10);
            foreach(var article in news){
                Console.WriteLine();
                Console.WriteLine(article);
            }
        }
        
        private static async Task TestSectorPerformance(IIEXCloudClient client)
        {
            var sectorPerformances = await client.GetSectorPerformances();
            foreach(var sectorPerformance in sectorPerformances){
                Console.WriteLine();
                Console.WriteLine(sectorPerformance);
            }
        }

        private static async Task TestHistoricalPrices(IIEXCloudClient client)
        {
            var prices = await client.GetHistoricalPrices(Symbol, HistoricalPricesRange.OneMonth);
            foreach(var price in prices){
                Console.WriteLine();
                Console.WriteLine(price);
            }
        }

        private static async Task TestApiStatus(IIEXCloudClient client)
        {
            var status = await client.GetApiStatus();
            Console.WriteLine("api up: " + status.StatusUp);
            Console.WriteLine("api time: " + status.Time);
        }

        private static async Task TestOHLC(IIEXCloudClient client)
        {
            var ohlc = await client.GetOHLC(Symbol);
            Console.WriteLine(ohlc);
        }

        private static async Task TestCompany(IIEXCloudClient client)
        {
            var companyAapl = await client.GetCompany(Symbol);
            //Console.WriteLine("aapl last dividend: "+companyAapl.Dividends.OrderBy(x=>x.PaymentDate).Last().Amount);
            Console.WriteLine("aapl website: " + companyAapl.Website);
            //Console.WriteLine("aapl CurrentLongTermDebt: "+companyAapl.BalanceSheets.OrderBy(x=>x.ReportDate).Last().CurrentLongTermDebt);
        }

        private static async Task TestQuote(IIEXCloudClient client)
        {
            var aapleQuote = await client.GetQuote(Symbol);
            Console.WriteLine("aapl high: " + aapleQuote.High);
            Console.WriteLine("aapl low: " + aapleQuote.Low);
            Console.WriteLine("aapl close: " + aapleQuote.Close);
        }
    }


}
