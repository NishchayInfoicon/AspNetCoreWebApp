using Microsoft.AspNetCore.Mvc;
using Practice.Foundation.Infrastructure.Responses;
using Practice.Services.APIManagers;
using Practice.Services.APIManagers.Records;
using System.Data;

namespace Practice.Services.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostgresCRUDController : ControllerBase
    {

        private readonly ILogger<PostgresCRUDController> _logger;
        private readonly PostgresManager _postgresManager;

        public PostgresCRUDController(PostgresManager postgresManager)
        {
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
        public ResponseStatus UpsertCustomer([FromBody] Customer customer)
        {
            ResponseStatus response = new ResponseStatus();
            response = _postgresManager.UpsertRecord<Customer>(customer);
            return response;
        }

        [HttpGet]
        [Route("gettabledata")]
        public List<Customer> Read(string tablename)
        {
            return _postgresManager.ReadDataFromTable<Customer>(tablename);
        }
        [HttpGet]
        [Route("isdbexists")]
        public bool IsDBExists(string dbName)
        {
            return _postgresManager.IsDatabaseExist(dbName);
        }

        [HttpGet]
        [Route("istableexists")]
        public bool IsTableExists(string tablename)
        {
            return _postgresManager.IsTableExists(tablename);
        }
    }
}