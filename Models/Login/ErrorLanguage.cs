using System.Text.Json.Serialization;

namespace sync_swagger
{
    public class ErrorLanguage
    {
        [JsonPropertyName("ru")]
        public string Ru { get; set; }
    }
}
