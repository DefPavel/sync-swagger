﻿using System.Text.Json.Serialization;

namespace sync_swagger.Models.Personnel
{
    public class Rewarding
    {
        [JsonPropertyName("id_person")]
        public int PersonId { get; set; }

        [JsonPropertyName("type_name")]
        public string Type { get; set; } = "Награждение";

        [JsonPropertyName("type_order")]
        public string TypeOrder { get; set; } = "Награждение";

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("num_doc")]
        public string NumberDocument { get; internal set; }
        [JsonPropertyName("order")]
        public string OrderName { get; internal set; }
        [JsonPropertyName("date_order")]
        public string DateOrder { get; internal set; }
        [JsonPropertyName("date_issuing")]
        public string DateIssuing { get; internal set; }
    }
}