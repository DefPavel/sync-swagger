using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sync_swagger.Models.Personnel;
using sync_swagger.Service.Api;
using sync_swagger.Service.Firebird;
using System.Net.Http;
using System.Threading.Tasks;

namespace sync_swagger.Controllers
{
    [ApiController]
    public class SyncController
    {
        private readonly HttpClient _httpClient = new();

        private readonly ILogger<SyncController> _logger;
        public SyncController(ILogger<SyncController> logger)
        {
            _logger = logger;
        }

        // Синхонизация из Firebird в Postgres (Все Отделы и их должностя)
        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<Department>> SyncDepartment(string token)
        {
            ApiService api = new(_httpClient);
            //Получить данные отделов из Firebird
            var departments = await FirebirdService.GetDepartment();
            // Если запрос пустой
            if(departments == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer
            return await api.PostArrayByTokenAsync(@"/auth", token, departments);
        }
    }
}
