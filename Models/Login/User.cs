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
        public string GroupName { get; set; } // К какой группе пользователей относиться пользователь

        [JsonPropertyName("grants")]
        public Grants[] Grants { get; set; }// Права доступа пользователя

        [JsonPropertyName("code")]
        public System.Net.HttpStatusCode StatusCode { get; set; } // Статус запроса

        [JsonPropertyName("resp")]
        public ErrorLanguage Error { get; set; } // Вывод об ошибки 

        public string FullName => $"{UserName}@{GroupName}";



    }
}
