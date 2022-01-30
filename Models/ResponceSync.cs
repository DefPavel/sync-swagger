using System.Text.Json.Serialization;

namespace sync_swagger
{
    public class ResponceSync
    {
        [JsonPropertyName("success")]
        public int Success { get; set; } = 0;
        [JsonPropertyName("updated")]
        public int Updated { get; set; } = 0;
        [JsonPropertyName("error")]
        public int Failed { get; set; } = 0;
        [JsonPropertyName("details")]
        public string[] Details = System.Array.Empty<string>();
    }
}
