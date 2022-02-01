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
        public async Task<ActionResult<GlobalArray>> DepartmentAndPosition(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayDepartments = await FirebirdService.GetDepartment();
            // Если запрос пустой
            if (globalArray.ArrayDepartments == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/tree/sync", token, globalArray);
        }

        // Синхонизация из Firebird в Postgres (Все Сотрудники)
        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Persons(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayPersons = await FirebirdService.GetPersonsAsync();
            // Если запрос пустой
            if (globalArray.ArrayPersons == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/person/sync", token, globalArray);
        }
    }
}
