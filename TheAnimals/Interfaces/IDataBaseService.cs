using System.Data;

namespace TheAnimals.Interfaces
{
    public interface IDatabaseService
    {
        Task<DataTable> ExecuteQueryAsync(string query);
    }
}
