using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;

namespace EHContacts.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hostsettings.json", optional: true)
            .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(GetContentRootPath())
                .UseConfiguration(config)
                .UseEnvironment("Development")
                .UseStartup<EHContacts.Startup>();
              

            _testServer = new TestServer(builder);

            Client = _testServer.CreateClient();
        }

       

        private string GetContentRootPath()
        {
   
            var rootpath = Path.Combine(ApplicationEnvironment.ApplicationBasePath, @"..\..\..\..\..\EHContacts\EHContacts");
            return rootpath;
        }

        
        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
