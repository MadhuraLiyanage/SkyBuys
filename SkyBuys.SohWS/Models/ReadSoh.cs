using Newtonsoft.Json;
using SkyBuys.Enum.Enum;
using SkyBuys.Models;

namespace SkyBuys.SohWS
{
    public class ReadSoh
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(GlobalStaticVaiables.SohURI)
        };

        public async Task<IEnumerable<Soh>> GetSoh(string organizationID)
        {
            TextLogger.LogToText(LoogerType.Information, $"Extracting SOH for : {organizationID}");
            TextLogger.LogToText(LoogerType.Information, $"Call RESTful API enpoint [GET] {GlobalStaticVaiables.SohURI}{GlobalStaticVaiables.SohMethod}");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("token", GlobalStaticVaiables.ApiToken);
            _httpClient.DefaultRequestHeaders.Add("fusionOrganizationCode", organizationID);

            try 
            { 
                var result = await _httpClient.GetAsync(GlobalStaticVaiables.SohMethod);

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();

                    IEnumerable<Soh> soh = JsonConvert.DeserializeObject<IEnumerable<Soh>>(json);
                    TextLogger.LogToText(LoogerType.Information, $"Received response : {result.StatusCode}");

                    return soh;
                }
                else
                {
                    TextLogger.LogToText(LoogerType.Warning, $"Received response : {result.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error calling GetFutionSoh method. Exception : {ex.Message}");
            }
            return null;
        }
    }
}
