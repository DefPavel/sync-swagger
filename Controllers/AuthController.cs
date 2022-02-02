using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sync_swagger.Service;
using sync_swagger.Service.Api;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace sync_swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _httpClient = new();

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<User> Auth(string username = "1978", string password = "root")
        {         
            ApiService api = new(_httpClient);

            User user = new()
            {
                UserName = username,
                Password = CustomAes256.Encrypt(password, "8UHjPgXZzXDgkhqV2QCnooyJyxUzfJrO"),
                IdModules = ModulesProject.Personel,

            };

            return await api.PostAsync(@"/auth", user);
        }

    }
}
