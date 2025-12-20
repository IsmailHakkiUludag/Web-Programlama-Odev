using System.Text;
using Newtonsoft.Json;

namespace SporSalonuYonetim.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;

    
        private readonly string _apiKey = "AIzaSyAuuppkIunrotyBM6Fwvg62MBnn6RkPn_c";

      
        private readonly string _baseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

        public GeminiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> ProgramOlustur(string cinsiyet, int yas, double kilo, double boy, string hedef)
        {
            var prompt = $"Sen profesyonel bir antrenörsün. Üye: {cinsiyet}, {yas} yaş, {kilo}kg, {boy}cm. Hedef: {hedef}. " +
                         "Bana 4 haftalık beslenme ve antrenman programı yaz. " +
                         "Sadece HTML formatında (h3, ul, li etiketleri kullanarak) cevap ver. Asla Markdown (```) kullanma.";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}?key={_apiKey}", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return $@"
                        <div class='alert alert-danger'>
                            <strong>Hata!</strong><br>
                            Model: gemini-2.5-flash<br>
                            Durum: {response.StatusCode}<br>
                            Detay: {errorDetails}
                        </div>";
                }

                var jsonResult = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(jsonResult);

                if (result.candidates != null && result.candidates.Count > 0)
                {
                    return result.candidates[0].content.parts[0].text;
                }

                return "<div class='alert alert-warning'>Yapay zeka boş cevap döndü.</div>";
            }
            catch (Exception ex)
            {
                return $"<div class='alert alert-danger'>Bağlantı Hatası: {ex.Message}</div>";
            }
        }
    }

}
