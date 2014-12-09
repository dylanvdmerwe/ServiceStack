using System;
using System.Net;
using System.Threading;
using NUnit.Framework;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.WebHost.Endpoints.Tests.Support.Host;

namespace ServiceStack.WebHost.Endpoints.Tests
{
	public abstract class AsyncServiceClientTests
	{
		protected const string ListeningOn = "http://localhost:82/";

		ExampleAppHostHttpListener appHost;

		[TestFixtureSetUp]
		public void OnTestFixtureSetUp()
		{
			appHost = new ExampleAppHostHttpListener();
			appHost.Init();
			appHost.Start(ListeningOn);
		}

		[TestFixtureTearDown]
		public void OnTestFixtureTearDown()
		{
			appHost.Dispose();
		}

		protected abstract IServiceClient CreateServiceClient();

		private static void FailOnAsyncError<T>(T response, Exception ex)
		{
			Assert.Fail(ex.Message);
		}

		[Test]
		public void Can_call_SendAsync_on_ServiceClient()
		{
			var jsonClient = CreateServiceClient();

			var request = new GetFactorial { ForNumber = 3 };
			GetFactorialResponse response = null;
			jsonClient.SendAsync<GetFactorialResponse>(request, r => response = r, FailOnAsyncError);

			Thread.Sleep(1000);

			Assert.That(response, Is.Not.Null, "No response received");
			Assert.That(response.Result, Is.EqualTo(GetFactorialService.GetFactorial(request.ForNumber)));
		}

        [Test]
        public async void Can_use_await_get_on_ServiceClient()
        {
            var client = CreateServiceClient();

            var request = new GetFactorial { ForNumber = 3 };
            GetFactorialResponse response = null;
            response = await client.GetAsync(request);

            Assert.That(response, Is.Not.Null, "No response received");
            Assert.That(response.Result, Is.EqualTo(GetFactorialService.GetFactorial(request.ForNumber)));
        }


        [Test]
        public async void Can_use_await_get_on_ServiceClient_NewRoute()
        {
            JsonServiceClient client = (JsonServiceClient)CreateServiceClient();
            client.UseNewPredefinedRoutes = true;

            var request = new GetFactorial2 { ForNumber = 3 };
            GetFactorialResponse response = null;
            response = await client.GetAsync(request);

            Assert.That(response, Is.Not.Null, "No response received");
            Assert.That(response.Result, Is.EqualTo(GetFactorialService.GetFactorial(request.ForNumber)));
        }

        [Test]
        public async void Can_use_await_post_on_ServiceClient()
        {
            IServiceClient client = CreateServiceClient();

            var request = new GetFactorial { ForNumber = 8 };
            GetFactorialResponse response = null;
            response = await client.PostAsync(request);

            Assert.That(response, Is.Not.Null, "No response received");
            Assert.That(response.Result, Is.EqualTo(GetFactorialService.GetFactorial(request.ForNumber)));
        }

        [Test]
        public async void Can_use_await_post_timeout_on_ServiceClient()
        {
            JsonServiceClient client = new JsonServiceClient(ListeningOn);
            client.Timeout = TimeSpan.FromSeconds(2);
            
            var request = new GetFactorial { ForNumber = 999 };
            GetFactorialResponse response = null;
            try
            {
                response = await client.PostAsync(request);
            }
            catch (Exception ex)
            {
                Assert.That(ex is WebException);
                Assert.That(((WebException)ex).Status, Is.EqualTo(WebExceptionStatus.Timeout));
            }
        }

		[TestFixture]
		public class JsonAsyncServiceClientTests : AsyncServiceClientTests
		{
			protected override IServiceClient CreateServiceClient()
			{
			    return new JsonServiceClient(ListeningOn);
			}
		}

		[TestFixture]
		public class JsvAsyncServiceClientTests : AsyncServiceClientTests
		{
			protected override IServiceClient CreateServiceClient()
			{
				return new JsvServiceClient(ListeningOn);
			}
		}

		[TestFixture]
		public class XmlAsyncServiceClientTests : AsyncServiceClientTests
		{
			protected override IServiceClient CreateServiceClient()
			{
				return new XmlServiceClient(ListeningOn);
			}
		}
	}


}