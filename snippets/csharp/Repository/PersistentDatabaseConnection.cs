public class PersistentDatabaseConnection : DatabaseConnection
{
    public List<MySqlCommand> Commands { get; set; } = new();

    public PersistentDatabaseConnection(IConfigs configs) : base(configs)
    {

    }

    public async Task<bool> ModifyWithTransactionAsync()
    {
        // setup a new database connection object
        using MySqlConnection connection = GetNewConnection();
        await connection.OpenAsync();

        // start up a transaction
        using MySqlTransaction transaction = await connection.BeginTransactionAsync();

        try
        {
            foreach (MySqlCommand command in Commands)
            {
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            await transaction.DisposeAsync();
            await CloseConnectionAsync(connection);
        }

        catch (MySqlException ex)
        {
            await transaction.RollbackAsync();
            await transaction.DisposeAsync();
            await CloseConnectionAsync(connection);

            if (ex.Number == USER_DEFINED_EXCEPTION_NUMBER)
            {
                throw new RepositoryException(ex);
            }

            throw;
        }

        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            await transaction.DisposeAsync();
            await CloseConnectionAsync(connection);

            throw;
        }

        return true;
    }



}