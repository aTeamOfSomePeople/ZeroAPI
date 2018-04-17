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
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44364") };

        public static async Task<long?> UploadFile(string path) => await UploadFile(File.Open(path, FileMode.Open));
        public static async Task<long?> UploadFile(byte[] fileBytes) => await UploadFile(new MemoryStream(fileBytes));
        public static async Task<long?> UploadFile(Stream fileStream)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(fileStream), "file", ".jpg");

            var httpResponse = await httpClient.PostAsync($"files/uploadfile", content);
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
