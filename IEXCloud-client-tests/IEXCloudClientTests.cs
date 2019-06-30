using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IEXCloudClient.NetCore;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IEXCloudClient.NetCore.Tests
{
    public class IEXCloudClientTests
    {
        private IIEXCloudClient client;
        private HttpResponseMessage responseMessage;
        private Mock<IHttpClientFactory> httpClientFactoryMock;

        [SetUp]
        public void Setup()
        {
            httpClientFactoryMock = new Mock<IHttpClientFactory>(); 

            //TODO it would be nicer if the setup of the httpclient and the SUT would be here and not in each test
        }
        
        [Test]
        public async Task TestSystemEvents()
        {
            var unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            var eventObj = new
            {
                systemEvent = "R",
                timestamp = unixTimestamp
            };

            SetupJsonResponse(eventObj);
            SetupHttpClientMock();
            SetupSUT();

            var result = await client.GetSystemEvents();
            Assert.That(result.Type, Is.EqualTo(SystemEventType.StartOfRegularMarketHours));
            Assert.That(result.Time.Date, Is.EqualTo(DateTime.UtcNow.Date));
        }

        [Test]
        public async Task TestStatus()
        {
            var status = new
            {
                Status = "up"
            };
            SetupJsonResponse(status);
            SetupHttpClientMock();
            SetupSUT();

            var result = await client.GetApiStatus();
            Assert.That(result.StatusUp);
        }

        [Test]
        public async Task TestGetHistoricalPrices()
        {
            var json = @"
            [
                {
                    ""date"": ""2017-04-03"",
                    ""open"": 143.1192,
                    ""high"": 143.5275,
                    ""low"": 142.4619,
                    ""close"": 143.1092,
                    ""volume"": 19985714,
                    ""uOpen"": 143.1192,
                    ""uHigh"": 143.5275,
                    ""uLow"": 142.4619,
                    ""uClose"": 143.1092,
                    ""uVolume"": 19985714,
                    ""change"": 0.039835,
                    ""changePercent"": 0.028,
                    ""label"": ""Apr 03, 17"",
                    ""changeOverTime"": -0.0039
                } 
            ]";
            SetupJsonResponseString(json);
            SetupHttpClientMock();
            SetupSUT();

            var result = await client.GetHistoricalPrices("sym", HistoricalPricesRange.OneMonth);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Date.Year, Is.EqualTo(2017));
        }

        [Test]
        public async Task TestThrottling()
        {
            var status = new
            {
                Status = "up"
            };
            SetupJsonResponse(status);
            SetupHttpClientMock();
            SetupSUT();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 3; i++)
            {
                var result = await client.GetApiStatus();
            }            
            sw.Stop();

            //3 requests, 2 times waiting
            Assert.That(sw.Elapsed.TotalMilliseconds, Is.GreaterThanOrEqualTo(120));
        }

        private void SetupJsonResponse(object obj)
        {
            SetupJsonResponseString(JsonConvert.SerializeObject(obj));
        }

        private void SetupJsonResponseString(string json)
        {
            responseMessage = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
        
        private void SetupSUT()
        {
            var env = IEXCloudClientOptions.IEXCloudClientOptionsEnvironment.Sandbox;
            var version = IEXCloudClientOptions.IEXCloudClientOptionsVersion.V1;
            var options = new IEXCloudClientOptions(env, version, "pk", "st");
            client = new IEXCloudClient(options, httpClientFactoryMock.Object);
        }

        private void SetupHttpClientMock()
        {
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(() =>
            {
                var fakeHttpMessageHandler = new FakeHttpMessageHandler(responseMessage);
                var fakeHttpClient = new HttpClient(fakeHttpMessageHandler);
                return fakeHttpClient;
            });
        }
    }
    
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        private HttpResponseMessage _fakeResponse;

        public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_fakeResponse);
        }
    }
}