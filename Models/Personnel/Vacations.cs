﻿using System.Text.Json.Serialization;

namespace sync_swagger.Models.Personnel
{
    public class Vacations
    {
        [JsonPropertyName("pers_id")]
        public int PersonId { get; set; }
        [JsonPropertyName("type")]
        public string TypeVacation { get; set; }
        [JsonPropertyName("period")]
        public string Period { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("residue")]
        public int Ostatok { get; set; }

        [JsonPropertyName("order")]
        public string OrderName { get; set; }

        [JsonPropertyName("date_order")]
        public string DateOrder { get; set; }

        [JsonPropertyName("type_order")]
        public string TypeOrder { get; set; } = "Отпуск";

        [JsonPropertyName("date_start")]
        public string DateStart { get; set; }

        [JsonPropertyName("date_end")]
        public string DateEnd { get; set; }

    }
}