using Newtonsoft.Json;
using SkyBuys.Enum.Enum;
using SkyBuys.ImagesWS.Services;
using SkyBuys.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SkyBuys.ImagesWS
{
    public class ProcessSkyBuysFile
    {
        private readonly ISkyBuysRepository _skyBuysRepository = new SkyBuysRepository();

        public async void SendSkyBuysFile()
        {
            TextLogger.LogToText(LoogerType.Information, "Image upload started");

            IEnumerable<SkyBuysItem> skyBuysItems = _skyBuysRepository.GetSkyBuysItems();
            TextLogger.LogToText(LoogerType.Information, "Image definitions extracted from DB successfully");

            //login to skybuys
            try
            {
                SkyBuysLoginResults skyBuysLoginResults = await SkybuysLoginAsync();

                foreach (SkyBuysItem skyBuysItem in skyBuysItems)
                {
                    if (File.Exists(GlobalStaticVaiables.SkyBuysFilePath + $"{skyBuysItem.Sku}.jpg"))
                    {
                        try
                        {
                            await SkybuysImageAsync(skyBuysLoginResults, $"{skyBuysItem.Sku}.jpg");
                        }
                        catch (Exception ex)
                        {
                            //if image file not found, skip
                            TextLogger.LogToText(LoogerType.Information, $"Image definitions extraction error. Exception : {ex.Message}");
                        }
                    }
                }

                skyBuysLoginResults = null;
                skyBuysItems = null;
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Information, $"Login error occured while uploading images. Exception : {ex.Message}");
            }

        }

        //Call SkyBuys login API
        public async Task<SkyBuysLoginResults> SkybuysLoginAsync()
        {
            TextLogger.LogToText(LoogerType.Information, "SkyBuys login initiated");

            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                httpClient.BaseAddress = new Uri(GlobalStaticVaiables.SkyBuysApiBaseUrl);
                httpClient.DefaultRequestHeaders.Clear();

                SkyBuysLoginRequest skyBuysLoginRequest = new SkyBuysLoginRequest();
                skyBuysLoginRequest.Email = GlobalStaticVaiables.SkyBuysApiLoginName;
                skyBuysLoginRequest.Password = GlobalStaticVaiables.SkyBuysApiPassword;

                var result = await httpClient.PostAsJsonAsync(GlobalStaticVaiables.SkyBuysApiLoginEndpoint, skyBuysLoginRequest);
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();

                    SkyBuysLoginResults skyBuysLoginResults = JsonConvert.DeserializeObject<SkyBuysLoginResults>(json);
                    TextLogger.LogToText(LoogerType.Information, $"Received response : {skyBuysLoginResults}");
                    return skyBuysLoginResults;
                }
                else
                {
                    TextLogger.LogToText(LoogerType.Warning, $"Received response : {result.StatusCode}. Error in login to SkyBuys API.");
                    return null;
                }

            }
        }

        //Call SkyBuys Product API
        public async Task<bool> SkybuysImageAsync(SkyBuysLoginResults skyBuysLoginResults, string fileName)
        {
            TextLogger.LogToText(LoogerType.Information, "SkyBuys Image files uploading initiated");

            //buypass https issues
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                httpClient.BaseAddress = new Uri(GlobalStaticVaiables.SkyBuysApiBaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                httpClient.DefaultRequestHeaders.Add("UserId", skyBuysLoginResults.Data.Id);
                httpClient.DefaultRequestHeaders.Add("Authorization", skyBuysLoginResults.Data.LoginToken);
                httpClient.DefaultRequestHeaders.Add("AccessToken", skyBuysLoginResults.Data.AccessToken);

                //cache
                CacheControlHeaderValue cacheControl = new CacheControlHeaderValue();
                cacheControl.NoCache = true;
                httpClient.DefaultRequestHeaders.CacheControl = cacheControl;

                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    byte[] bytes = File.ReadAllBytes(GlobalStaticVaiables.SkyBuysFilePath + fileName);
                    HttpContent fileContent = new ByteArrayContent(bytes);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                    //Add the SkyBuys Product file
                    multipartFormContent.Add(fileContent, "images", fileName);

                    var result = await httpClient.PostAsync(GlobalStaticVaiables.SkyBuysImageEndpoint, multipartFormContent);

                    if (result.IsSuccessStatusCode)
                    {
                        TextLogger.LogToText(LoogerType.Information, $"Item image file {fileName} submitted sucessfully.");
                        httpClientHandler = null;
                        return true;
                    }
                    else
                    {
                        TextLogger.LogToText(LoogerType.Warning, $"Error submitting Item image file {fileName} CSV file. Response : {result.StatusCode}");
                        httpClientHandler = null;
                        return false;
                    }
                }
            } 

        }
    }
}

