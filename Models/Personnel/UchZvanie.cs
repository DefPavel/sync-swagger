using System.Text.Json.Serialization;

namespace sync_swagger.Models.Personnel
{
    public class UchZvanie
    {
        [JsonPropertyName("id_person")]
        public int IdPerson { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("date_issue")]
        public string DateDateIssue { get; set; }

        [JsonPropertyName("document")]
        public string Document { get; set; }

        [JsonPropertyName("place")]
        public string Place { get; set; }

    }
}