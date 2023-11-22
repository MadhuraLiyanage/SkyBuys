using Newtonsoft.Json;
using SkyBuys.Enum.Enum;
using SkyBuys.Models;
using SkyBuys.PromoWS.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SkyBuys.PromoWS
{
    public class ProcessSkyBuysFile
    {
        private readonly ISkyBuysRepository _skyBuysRepository = new SkyBuysRepository();

        public async void SendSkyBuysFile()
        {
            /*bool productExtraction;*/
            bool promoExtraction;

            /*productExtraction =  GenerateProductFile();*/
            promoExtraction = GeneratePromoFile();

            if (promoExtraction)
            {
                SkyBuysLoginResults skyBuysLoginResults=null;
                try
                {
                    //Skybuys call Login API
                    skyBuysLoginResults = await SkybuysLoginAsync();

                    try
                    {
                        //Sending the prodcut file
                        await SkybuysPromoAsync(skyBuysLoginResults);

                    }
                    catch (Exception ex)
                    {
                        TextLogger.LogToText(LoogerType.Error, "Error uploading Skybuys Promo file.");
                    }
                }
                catch (Exception ex)
                {
                    TextLogger.LogToText(LoogerType.Error, "Error validating Skybuys user.");
                }
            }

        }

        //Generate SkyBuys Promo CSV file
        public bool GeneratePromoFile()
        {
            TextLogger.LogToText(LoogerType.Information, "Promo extraction started");
            
            try
            {
                //generate Promo file
                IEnumerable<SkyBuysPromo> skyBuysPromos = _skyBuysRepository.GetSkyBuysPromo();
                TextLogger.LogToText(LoogerType.Information, "Promo data extracted successfully");
                //create CSV file
                TextLogger.LogToText(LoogerType.Information, "Promo CSV file building started");
                using (StreamWriter sw = new StreamWriter(GlobalStaticVaiables.SkyBuysFilePath + GlobalStaticVaiables.SkyBuysPromFileName))
                {
                    sw.WriteLine($"Promotion ID,On sale from,On sale to,Promotion description,SKU,Standard sales price,Promotion sales price");
                    foreach (SkyBuysPromo skyBuysPromo in skyBuysPromos)
                    {
                        sw.WriteLine($"{skyBuysPromo.PromotionId},{skyBuysPromo.PromotionType},{skyBuysPromo.OnSaleFrom}," +
                            $"{skyBuysPromo.OnSaleTo},{skyBuysPromo.PromotionDescription.Replace("\n", "").Replace("\r", "")},{skyBuysPromo.Sku},{skyBuysPromo.StandardSalesPrice}," +
                            $"{skyBuysPromo.PromotionSalesPrice}");
                    }
                }
                TextLogger.LogToText(LoogerType.Information, "Promo CSV file building completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error Processing Promo CSV File. Exception : {ex.Message}");
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
                    TextLogger.LogToText(LoogerType.Information, $"Received response : {JsonConvert.SerializeObject(skyBuysLoginResults)}");
                    return skyBuysLoginResults;
                }
                else
                {
                    TextLogger.LogToText(LoogerType.Warning, $"Received response : {result.StatusCode}. Error in login to SkyBuys API.");
                    return null;
                }

            }
        }

        //Call SkyBuys Promo API
        public async Task<bool> SkybuysPromoAsync(SkyBuysLoginResults skyBuysLoginResults)
        {
            TextLogger.LogToText(LoogerType.Information, "SkyBuys Promo File Uploading initiated");

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
                    byte[] bytes = File.ReadAllBytes(GlobalStaticVaiables.SkyBuysFilePath + GlobalStaticVaiables.SkyBuysPromFileName);
                    HttpContent fileContent = new ByteArrayContent(bytes);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");

                    //Add the SkyBuys Product file
                    multipartFormContent.Add(fileContent, "offers", GlobalStaticVaiables.SkyBuysPromFileName);

                    var result = await httpClient.PostAsync(GlobalStaticVaiables.SkyBuysOffersEndpoint, multipartFormContent);

                    if (result.IsSuccessStatusCode)
                    {
                        TextLogger.LogToText(LoogerType.Information, "Promo CSV file submitted sucessfully.");
                        return true;
                    }
                    else
                    {
                        TextLogger.LogToText(LoogerType.Warning, $"Error submitting Promo CSV file. Response : {result.StatusCode}");
                        return false;
                    }
                }
            }
        }
    }
}

