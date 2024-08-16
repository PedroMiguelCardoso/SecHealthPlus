using MySql.Data.MySqlClient;
using System.Data;
using TheAnimals.Interfaces;

namespace TheAnimals.Services
{
    public class DatabaseService : IDatabaseService //Interface do serviço
    {
        private readonly IConfiguration _configuration;  // Dependências do serviço padrão do C#
        private readonly ILogger<DatabaseService> _logger; // Dependência pra habilitar comentário via prompt

        public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger) // Construtor com dependências (
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<DataTable> ExecuteQueryAsync(string query) // Realiza a consulta da tabela
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            DataTable dataTable = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connectionString)) // Gerando conexão com mysql 
            {
                try
                {
                    await conn.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))    // Recebe a query do banco 
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing query: {Query}", query);  //Comenta o erro
                    throw;
                }
            }

            return dataTable;
        }
    }
}
