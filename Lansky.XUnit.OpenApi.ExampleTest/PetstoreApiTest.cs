using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Lansky.XUnit.OpenApi.ExampleTest
{
    public class PetstoreApiTest
    {
        [Theory]
        [AllOperations("petstore.yaml")]
        public async Task EveryOperationNeedsAuthentication(HttpMethod method, string path)
        {
            var response = await new HttpClient().SendAsync(new HttpRequestMessage(method, $"https://server.under.test/{path}"));
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
