using Newtonsoft.Json;
using SkyBuys.Enum.Enum;
using SkyBuys.Models;
using SkyBuys.ProductWS.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SkyBuys.ProductWS
{
    public class ProcessSkyBuysFile
    {
        private readonly ISkyBuysRepository _skyBuysRepository = new SkyBuysRepository();

        public async void SendSkyBuysFile()
        {
            bool productExtraction;
   
            productExtraction =  GenerateProductFile();

            if (productExtraction)
            {
                SkyBuysLoginResults skyBuysLoginResults=null;
                try
                {
                    //Skybuys call Login API
                    skyBuysLoginResults = await SkybuysLoginAsync();

                    try
                    {
                        //Sending the prodcut file
                        await SkybuysProdutAsync(skyBuysLoginResults);

                    }
                    catch(Exception ex)
                    {
                        TextLogger.LogToText(LoogerType.Error, "Error uploading Skybuys Product file.");
                    }
                }
                catch (Exception ex)
                {
                    TextLogger.LogToText(LoogerType.Error, "Error validating Skybuys user.");
                }
            }

        }

        //Generate SkyBuys product CSV file
        public bool GenerateProductFile()
        {
            TextLogger.LogToText(LoogerType.Information, "Product extraction started");

            try
            {
                IEnumerable<SkyBuysItem> skyBuysItems = _skyBuysRepository.GetSkyBuysItems();
                TextLogger.LogToText(LoogerType.Information, "Product data extracted successfully");
                //create CSV file
                TextLogger.LogToText(LoogerType.Information, "Product CSV file building started");

                using (StreamWriter sw = new StreamWriter(GlobalStaticVaiables.SkyBuysFilePath + GlobalStaticVaiables.SkyBuysFileName))
                {
                    sw.WriteLine($"Brand Name,Short Description,Long Description,Size,Image URL,SKU,RRP Price," +
                        $"Category,Sub Category,Location,Additional Product Image,Additional Product Video,Type,SOH");
                    foreach (SkyBuysItem skyBuysItem in skyBuysItems)
                    {
                        sw.WriteLine($"{skyBuysItem.Brand},{skyBuysItem.ShortDescription.Replace("\n", "").Replace("\r", "")},{skyBuysItem.LongDescription.Replace("\n", "").Replace("\r", "")}," +
                            $"{skyBuysItem.Size},{skyBuysItem.ImageURL},{skyBuysItem.Sku},{skyBuysItem.RrpPrice}," +
                            $"{skyBuysItem.Category},{skyBuysItem.SubCategory},{skyBuysItem.Location},{skyBuysItem.AdditionProductImage}," +
                            $"{skyBuysItem.AdditionalProductVideo},{skyBuysItem.Type},{skyBuysItem.SOH}");
                    }
                }
                TextLogger.LogToText(LoogerType.Information, "Product CSV file building completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error Processing Product CSV File. Exception : {ex.Message}");
                return false;
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
        public async Task<bool> SkybuysProdutAsync(SkyBuysLoginResults skyBuysLoginResults)
        {
            TextLogger.LogToText(LoogerType.Information, "SkyBuys Product File Uploading initiated");

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
                    byte[] bytes = File.ReadAllBytes(GlobalStaticVaiables.SkyBuysFilePath + GlobalStaticVaiables.SkyBuysFileName);
                    HttpContent fileContent = new ByteArrayContent(bytes);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");

                    //Add the SkyBuys Product file
                    multipartFormContent.Add(fileContent, "products", GlobalStaticVaiables.SkyBuysFileName);

                    var result = await httpClient.PostAsync(GlobalStaticVaiables.SkyBuysProdInvEndpoint, multipartFormContent);

                    if (result.IsSuccessStatusCode)
                    {
                        TextLogger.LogToText(LoogerType.Information, "Product CSV file submitted sucessfully.");
                        return true;
                    }
                    else
                    {
                        TextLogger.LogToText(LoogerType.Warning, $"Error submitting product CSV file. Response : {result.StatusCode}");
                        return false;
                    }
                }
            } 

        }
    }
}

