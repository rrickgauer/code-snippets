public class DatabaseConnection
{
    protected const int USER_DEFINED_EXCEPTION_NUMBER = 1644;
    protected readonly IConfigs _configs;

    public string ConnectionString => $"server={_configs.DbServer};user={_configs.DbUser};database={_configs.DbDataBase};password={_configs.DbPassword}";


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configs"></param>
    public DatabaseConnection(IConfigs configs)
    {
        _configs = configs;
    }


    /// <summary>
    /// Fetch the first row from a data table (one result).
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<DataRow?> FetchAsync(MySqlCommand command)
    {
        var dataTable = await FetchAllAsync(command);

        DataRow? row = null;

        if (dataTable.Rows.Count > 0)
        {
            row = dataTable.Rows[0];
        }

        return row;
    }


    /// <summary>
    /// Retrieve all the data rows for the specified sql command
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<DataTable> FetchAllAsync(MySqlCommand command)
    {
        // setup a new database connection object
        using MySqlConnection connection = GetNewConnection();

        await connection.OpenAsync();

        command.Connection = connection;

        // fill the datatable with the command results
        DataTable results = await RepositoryUtils.LoadDataTableAsync(command);

        // close the connection
        await CloseConnectionAsync(connection);

        return results;
    }



    public async Task<uint?> ModifyWithRowIdAsync(MySqlCommand command)
    {
        var numRecords = await ModifyAsync(command);

        if (numRecords < 1)
        {
            return null;
        }


        if (!uint.TryParse(command.LastInsertedId.ToString(), out var rowId))
        {
            return null;
        }

        return rowId;
    }


    /// <summary>
    /// Exeucte the specified sql command that modifies (insert, update, delete) data.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Number of affected rows</returns>
    public async Task<int> ModifyAsync(MySqlCommand command)
    {
        try
        {
            // setup a new database connection object
            using MySqlConnection connection = GetNewConnection();
            await connection.OpenAsync();
            command.Connection = connection;

            // execute the non query command
            int numRowsAffected = await command.ExecuteNonQueryAsync();

            // close the connection
            await CloseConnectionAsync(connection);

            return numRowsAffected;
        }
        catch(MySqlException ex)
        {
            if (ex.Number == USER_DEFINED_EXCEPTION_NUMBER)
            {
                throw new RepositoryException(ex);
            }

            throw;
        }

    }

    /// <summary>
    /// Get a new connection using the connection string
    /// </summary>
    /// <returns></returns>
    protected MySqlConnection GetNewConnection()
    {
        return new MySqlConnection(ConnectionString);
    }

    protected static async Task CloseConnectionAsync(MySqlConnection connection)
    {
        // close the connection
        await connection.CloseAsync();
        await connection.DisposeAsync();
    }
}