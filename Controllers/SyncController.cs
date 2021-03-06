using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sync_swagger.Models.Personnel;
using sync_swagger.Service.Api;
using sync_swagger.Service.Firebird;
using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace sync_swagger.Controllers
{
    [ApiController]
    public class SyncController
    {
        private const string Hosting = "http://localhost:8080";
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
            using ClientApi client = new(Hosting);
            GlobalArray globalArray = new()
            {
                //Получить данные отделов из Firebird
                ArrayDepartments = await FirebirdService.GetDepartment()
            };
            // Если запрос пустой
            return globalArray.ArrayDepartments.Count == 0
               ? new BadRequestResult()
               : await client.PostAsyncByToken<GlobalArray>(@"api/pers/tree/sync", token, globalArray);
        }

        // Синхонизация из Firebird в Postgres (Все Сотрудники)
        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Persons(string token)
        {
            using ClientApi client = new(Hosting);

            GlobalArray globalArray = new()
            {
                //Получить данные отделов из Firebird
                ArrayPersons = await FirebirdService.GetPersonsAsync()
            };
            /*using FileStream createStream = System.IO.File.Create(@"students.json");

            // Сериализация в UTF-8
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            await JsonSerializer.SerializeAsync(createStream, globalArray, options);
            */
       
            // Если запрос пустой
            return globalArray.ArrayPersons.Count == 0
                ? new BadRequestResult()
                : await client.PostAsyncByToken<GlobalArray>(@"api/pers/person/sync", token, globalArray);

            /*int countPerson = await FirebirdService.GetCountPersons();
            int skip = 0;
            
            for (int i = 0; i <= countPerson; i++)
            {
                globalArray.ArrayPersons = await FirebirdService.GetPersonsAsync(250, skip);
                // Если запрос пустой
                if (globalArray.ArrayPersons.Count > 0)
                {
                    // Отправить массив данных на ApiServer для синхронизации
                    await client.PostAsyncByToken<GlobalArray>(@"api/pers/person/sync", token, globalArray);
                    skip += 250;
                }
                else
                {
                    return new OkResult();
                }
            }
            return new OkResult();
            */

        }

        // Синхонизация из Firebird в Postgres (Все Отпуска)
        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Vacations(string token)
        {
            using ClientApi client = new(Hosting);
            
            GlobalArray globalArray = new()
            {
                //Получить данные отделов из Firebird
                ArrayVacation = await FirebirdService.GetVacations()
            };
            // Если запрос пустой
            return globalArray.ArrayVacation.Count == 0
                ? new BadRequestResult()
                : await client.PostAsyncByToken<GlobalArray>(@"api/pers/vacation/sync", token, globalArray);
        }

        // Синхонизация из Firebird в Postgres (Все Награждения)
        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Rewardings(string token)
        {
            using ClientApi client = new(Hosting);
            GlobalArray globalArray = new()
            {
                //Получить данные отделов из Firebird
                ArrayRewarding = await FirebirdService.GetRewarding()
            };
            // Если запрос пустой
            return globalArray.ArrayRewarding.Count == 0
                ? new BadRequestResult()
                : await client.PostAsyncByToken<GlobalArray>(@"api/pers/rewarding/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Qualifications(string token)
        {
            using ClientApi client = new(Hosting);
            GlobalArray globalArray = new()
            {
                //Получить данные отделов из Firebird
                ArrayQualification = await FirebirdService.GetQualification()
            };

            return globalArray.ArrayQualification.Count == 0
                ? new BadRequestResult()
                : await client.PostAsyncByToken<GlobalArray>(@"api/pers/qualification/sync", token, globalArray);
          
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> AcademicTitles(string token)
        {
            using ClientApi client = new(Hosting);
            GlobalArray globalArray = new();
            globalArray.ArrayAcademicTitle = await FirebirdService.GetUchZvanieList();
            return globalArray.ArrayAcademicTitle.Count == 0
                ? new BadRequestResult()
                : await client.PostAsyncByToken<GlobalArray>(@"api/pers/academicTitle/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> ScientificDegrees(string token)
        {
            using ClientApi client = new(Hosting);
            GlobalArray globalArray = new()
            {
                ArrayDegrees = await FirebirdService.GetScientificDegrees()
            };
            return globalArray.ArrayDegrees.Count == 0
                ? new BadRequestResult()
                : await client.PostAsyncByToken<GlobalArray>(@"api/pers/scientific/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Moves(string token)
        {
            using ClientApi client = new(Hosting);
            GlobalArray globalArray = new()
            {
                ArrayMove = await FirebirdService.GetMovesAsync()
            };

            return globalArray.ArrayMove.Count == 0
                ? new BadRequestResult()
                : await client.PostAsyncByToken<GlobalArray>(@"api/pers/relocation/sync", token, globalArray);
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Avatars(string token)
        {
            using ClientApi client = new(Hosting);
            var countPerson = await FirebirdService.GetCountPersons();
            var skip = 0;
            GlobalArray globalArray = new();
            for (var i = 0; i < countPerson; i++)
            {
                globalArray.ArrayImage = await FirebirdService.GetPhoto(100, skip);
                if (globalArray.ArrayImage.Count > 0)
                {
                    // Отправить массив данных на ApiServer для синхронизации
                    await client.PostAsyncByToken<GlobalArray>(@"api/pers/person/sync/image", token, globalArray);
                    skip += 100;
                }
                else
                {
                    return new OkResult();
                }
            }
            return new OkResult();
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult<GlobalArray>> Documents(string token)
        {
            using ClientApi client = new(Hosting);
            GlobalArray globalArray = new();
            var countDocuments = await FirebirdService.GetCountDocuments();
            var skip = 0;
            
            for (var i = 0; i < countDocuments; i++)
            {
                globalArray.ArrayDocuments = await FirebirdService.GetDocumentsAsync(100, skip);

                if (globalArray.ArrayDocuments.Count > 0)
                {
                    await client.PostAsyncByToken<GlobalArray>(@"api/pers/document/sync", token, globalArray);
                    skip += 100;
                }
                else
                {
                    return new OkResult();
                }
            }
            return new OkResult();
        }

    }
}
