using System.Text.Json.Serialization;

namespace sync_swagger.Models.Personnel
{
    public class Documents
    {
        [JsonPropertyName("id_person")]
        public int IdPers { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("document")]
        public byte[] Document { get; set; }
    }
}