using sync_swagger.Models.Login;
using System.Text.Json.Serialization;

namespace sync_swagger
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } // Id пользователя

        [JsonPropertyName("login")]
        public string UserName { get; set; } // Логин

        [JsonPropertyName("password")]
        public string Password { get; set; } // Пароль

        [JsonPropertyName("auth_token")]
        public string Token { get; set; } // Токен для middleware

        [JsonPropertyName("id_module")]
        public ModulesProject IdModules { get; set; } // Модуль программы

        [JsonPropertyName("group_name")]
        public Groups[] GroupName { get; set; } // К какой группе пользователей относиться пользователь

        [JsonPropertyName("grants")]
        public Grants[] Grants { get; set; }// Права доступа пользователя


        public string FullName => $"{UserName}@";



    }
}
