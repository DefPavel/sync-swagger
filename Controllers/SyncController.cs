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

        // Синхонизация из Firebird в Postgres (Все Отпуска)
        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Vacations(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayVacation = await FirebirdService.GetVacations();
            // Если запрос пустой
            if (globalArray.ArrayVacation == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/vacation/sync", token, globalArray);
        }

        // Синхонизация из Firebird в Postgres (Все Награждения)
        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Rewardings(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayRewarding = await FirebirdService.GetRewarding();
            // Если запрос пустой
            if (globalArray.ArrayRewarding == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/rewarding/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Qualifications(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayQualification = await FirebirdService.GetQualification();
            // Если запрос пустой
            if (globalArray.ArrayQualification == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/qualification/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> AcademicTitles(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayAcademicTitle = await FirebirdService.GetUchZvanieList();
            // Если запрос пустой
            if (globalArray.ArrayAcademicTitle == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/academicTitle/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> ScientificDegrees(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayDegrees = await FirebirdService.GetScientificDegrees();
            // Если запрос пустой
            if (globalArray.ArrayDegrees == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/scientific/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Moves(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayMove = await FirebirdService.GetMovesAsync();
            // Если запрос пустой
            if (globalArray.ArrayMove == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/relocation/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Avatars(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayImage = await FirebirdService.GetPhoto();
            // Если запрос пустой
            if (globalArray.ArrayImage == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/person/sync/image", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Documents(string token)
        {
            ApiService api = new(_httpClient);
            GlobalArray globalArray = new();
            //Получить данные отделов из Firebird
            globalArray.ArrayDocuments = await FirebirdService.GetDocumentsAsync();
            // Если запрос пустой
            if (globalArray.ArrayDocuments == null)
            {
                return new BadRequestResult();
            }
            // Отправить массив данных на ApiServer для синхронизации
            return await api.PostArrayByTokenAsync(@"/pers/document/sync", token, globalArray);
        }

    }
}
