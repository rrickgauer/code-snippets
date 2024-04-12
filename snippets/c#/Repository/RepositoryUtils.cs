public static class RepositoryUtils
{
    public static async Task<DataTable> LoadDataTableAsync(MySqlCommand command)
    {
        DataTable dataTable = new();

        DbDataReader reader = await command.ExecuteReaderAsync();
        dataTable.Load(reader);

        return dataTable;
    }
}