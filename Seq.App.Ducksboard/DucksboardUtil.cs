using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Seq.App.Ducksboard
{
    public static class DucksboardUtil
    {

        public static async Task SendData(string apiKey, string label, object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://push.ducksboard.com/v/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var byteArray = Encoding.ASCII.GetBytes(apiKey + ":");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var response = await client.PostAsJsonAsync(label, data);

                if (!response.IsSuccessStatusCode)
                {
                    Log
                        .ForContext("Uri", response.RequestMessage.RequestUri)
                        .Error("Could not push data to Ducksboard, server replied {StatusCode} {StatusMessage}: {Message}", Convert.ToInt32(response.StatusCode), response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }
        }

    }
}
