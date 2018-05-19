using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Files
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(Properties.Resources.ZeroMessenger) };

        public static async Task<long?> UploadFile(string path)
        {
            var file = File.Open(path, FileMode.Open);

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(file), "file", ".jpg");

            var httpResponse = await httpClient.PostAsync($"files/uploadfile", content);
            file.Close();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            
            try
            {
                return JsonConvert.DeserializeObject<long>(stringResponse);
            }
            catch
            {
                return null;
            }

        }
    }
}
