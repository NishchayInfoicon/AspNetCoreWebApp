using Microsoft.AspNetCore.Mvc;
using Practice.Foundation.Infrastructure.Responses;
using Practice.Services.APIManagers;
using Practice.Services.APIManagers.Records;

namespace Practice.Services.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostgresCRUDController : ControllerBase
    {

        private readonly ILogger<PostgresCRUDController> _logger;
        private readonly PostgresManager _postgresManager;

        public PostgresCRUDController(PostgresManager postgresManager, ILogger<PostgresCRUDController> logger)
        {
            _logger = logger;
            _postgresManager = postgresManager;
        }

        /// <summary>
        /// Type column name and data type in object 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createtable")]
        public ResponseStatus CreateTable(string tableName, Dictionary<string, string> columns)
        {
            ResponseStatus response = new ResponseStatus();
            response = _postgresManager.CreateTable(tableName, columns);
            return response;
        }

        [HttpPost]
        [Route("upsertcustomer")]
        public ResponseStatus UpsertCustomer([FromBody]Customer customer)
        {
            ResponseStatus response = new ResponseStatus();
            response = _postgresManager.UpsertRecord<Customer>(customer);
            return response;
        }
    }
}