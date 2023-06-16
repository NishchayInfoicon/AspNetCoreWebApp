namespace Practice.Client.Services.ClientManagers.Managers
{
    public class PostgresRepository
    {

        private readonly HttpClient _httpClient;
        public PostgresRepository(HttpClient httpClient)
        {

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7222/api/PostgresCRUD/");
        }


        public async Task UpsertRecord(string dbName)
        {
            try
            {
                var response =await _httpClient.GetAsync("isdbexists?dbName=customer");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
