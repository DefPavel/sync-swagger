using System.Text.Json.Serialization;

namespace sync_swagger.Models.Personnel
{
    public class Department : ResponceSync
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name_depart")]
        public string Name { get; set; }

        [JsonPropertyName("short")]
        public string Short { get; set; }

        [JsonPropertyName("parentId")]
        public int ParentId { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
