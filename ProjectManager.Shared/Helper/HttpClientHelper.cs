using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Shared.Helper
{
    public class HttpClientHelper
    {
        public async Task<T> GetAsync<T>(string baseUrl, string api, string token) where T : new()
        {
            string apiUrl = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(baseUrl)) throw new Exception("Base url is required");
                if (string.IsNullOrEmpty(api)) throw new Exception("API uri is required");

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Constants.ApplicationJson));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Constants.Bearer, token);

                    apiUrl = $"{baseUrl}/{api}";

                    if (apiUrl.Contains("https"))
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    }

                    var response = httpClient.GetAsync(apiUrl).Result;
                    if (response.IsSuccessStatusCode) //Successful
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        if (data == null || data.Length == 0)
                        {
                            throw new Exception($"No received anything yet.{Environment.NewLine}End point: [{baseUrl}]");
                        }

                        dynamic objData = JsonConvert.DeserializeObject<T>(data);
                        if (objData == null)
                        {
                            throw new Exception($"Can not deserialize data [{data}] to Json object");
                        }

                        return objData;
                    }
                    else
                    {
                        //Unsuccessful
                        throw new Exception($"The HTTP response was NOT successful. StatusCode = [{response.StatusCode}]");
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Send request to [{0}] failed.{1}Error: {2}", apiUrl, Environment.NewLine, ex.Message));
            }
        }

        public async Task<T> PostAsync<T>(object input, string baseUrl, string api, string token)
        {
            string apiUrl = string.Empty;
            try
            {
                if (input == null) throw new Exception("Input is required.");
                if (string.IsNullOrEmpty(baseUrl)) throw new Exception("Base url is required");
                if (string.IsNullOrEmpty(api)) throw new Exception("API uri is required");

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.Constants.ApplicationJson));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Constants.Bearer, token);

                    var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, Constants.Constants.ApplicationJson);
                    apiUrl = $"{baseUrl}/{api}";

                    if (apiUrl.Contains("https"))
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    }

                    var result = httpClient.PostAsync(apiUrl, content).Result;
                    if (result.IsSuccessStatusCode) //Successful
                    {
                        var data = await result.Content.ReadAsStringAsync();

                        if (data == null || data.Length == 0)
                        {
                            throw new Exception($"No received anything yet.{Environment.NewLine}End point: [{baseUrl}]");
                        }

                        dynamic objData = JsonConvert.DeserializeObject<T>(data);

                        if (objData == null)
                        {
                            throw new Exception($"Can not deserialize data [{data}] to Json object");
                        }
                        return objData;
                    }
                    else
                    {
                        //Unsuccessful
                        throw new Exception($"The HTTP response was NOT successful. StatusCode = [{result.StatusCode}]");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Send request to [{0}] failed.{1}Error: {2}", apiUrl, Environment.NewLine, ex.Message));
            }
        }
    }
}
