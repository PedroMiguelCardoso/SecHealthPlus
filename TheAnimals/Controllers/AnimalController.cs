using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using TheAnimals.Interfaces;

namespace TheAnimals.Controllers
{
    [ApiController]
    [Route("animals")]
    public class AnimalController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<AnimalController> _logger;

        public AnimalController(IDatabaseService databaseService, ILogger<AnimalController> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string query = "SELECT * FROM Animais_Peconhentos";
                DataTable animalsTable = await _databaseService.ExecuteQueryAsync(query);

                var resultList = new List<Dictionary<string, object>>();

                foreach (DataRow animalRow in animalsTable.Rows)
                {
                    var animalDict = animalsTable.Columns
                        .Cast<DataColumn>()
                        .ToDictionary(col => col.ColumnName, col => animalRow[col]);

                    int animalId = Convert.ToInt32(animalDict["Id_Animal"]);

                    // Obter sintomas
                    string sintomasQuery = $"SELECT Sintomas FROM sintomas WHERE Id_Animal = {animalId}";
                    DataTable sintomaTable = await _databaseService.ExecuteQueryAsync(sintomasQuery);

                    string sintomas = string.Join(", ", sintomaTable.AsEnumerable().Select(row => row.Field<string>("Sintomas")));

                    // Obter tratamento
                    string tratamentoQuery = $"SELECT Tratamento FROM tratamento WHERE Id_Animal = {animalId}";
                    DataTable tratamentoTable = await _databaseService.ExecuteQueryAsync(tratamentoQuery);

                    string tratamentos = string.Join(", ", tratamentoTable.AsEnumerable().Select(row => row.Field<string>("Tratamento")));

                    // Adicionar sintomas e tratamento ao dicionário do animal
                    animalDict["Sintomas"] = sintomas;
                    animalDict["Tratamento"] = tratamentos;

                    resultList.Add(animalDict);
                }

                string jsonResult = JsonConvert.SerializeObject(resultList, Formatting.Indented);

                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from database.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
