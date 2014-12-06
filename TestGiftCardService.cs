using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGiftCard;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GiftCertProTest
{
    [TestClass]
    public class TestGiftCardService
    {
        //http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        [TestMethod]
        public async void SalonLogin()
        {
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri("http://localhost:9000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.GetAsync("SalonLogin");
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.

                    // HTTP POST
                    var gizmo = new AuthModel() { Username = "test", Password = "Test2" };
                    response = await client.PostAsJsonAsync("SalonLogin/23", gizmo);
                    if (response.IsSuccessStatusCode)
                    {
                        // Get the URI of the created resource.
                        Uri gizmoUrl = response.Headers.Location;
                    }
                }
                catch (HttpRequestException e)
                {
                    // Handle exception.
                }
            }
        }
    }
}
