
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEXCloudClient.NetCore;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace IEXCloudClient.NetCore.Tests
{
    public class SignerTests
    {
        private INodeServices nodeServices;
        private ISigner mysigner;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddNodeServices();
            var serviceProvider = services.BuildServiceProvider();
            nodeServices = serviceProvider.GetRequiredService<INodeServices>();

            mysigner = new Signer();
        }
        
        [Test]
        public async Task Signer_should_return_same_result_as_reference_implementation()
        {
            //NOTE for this test to run successfully you need 
            // - Node.js installed locally (https://nodejs.org/en/download/)
            // - run in console:
            //      npm install --save moment

            const string Request_method = "GET";
            const string Canonical_uri = "v1/path";
            const string Canonical_querystring = "query=param";
            const string Access_key = "accesskey";
            const string Secret_key = "secretkey";
            const string Host = "host.com";
            
            var reference_result = await nodeServices.InvokeAsync<Dictionary<string, string>>("reference-signing.js", Request_method, Canonical_uri, Canonical_querystring, Access_key, Secret_key, Host);
            var myresult = mysigner.GenerateSigningData(Request_method, Canonical_uri, Canonical_querystring, Access_key, Secret_key, Host);

            Assert.That(reference_result["Authorization"], Is.EqualTo(myresult.authorization_header));
            Assert.That(reference_result["x-iex-date"], Is.EqualTo(myresult.iexdate));
        }
    }
}