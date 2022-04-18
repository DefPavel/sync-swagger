using System.Text.Json.Serialization;

namespace sync_swagger
{
    public class Grants
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

    }
}
