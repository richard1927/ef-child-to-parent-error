using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace error_repro;

public class SqlServerContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();


        await using var connection = new SqlConnection(_msSqlContainer.GetConnectionString());
        await connection.OpenAsync();

        const string createTablesCommand = """
                       CREATE TABLE Parents (
                           Id INT PRIMARY KEY,
                           Name NVARCHAR(100)
                       );
                       CREATE TABLE Children (
                           Id INT PRIMARY KEY,
                           ParentId INT,
                           Name NVARCHAR(100),
                           FOREIGN KEY (ParentId) REFERENCES Parents(Id)
                       );
                   
           """;

        await using var command = new SqlCommand(createTablesCommand, connection);
        await command.ExecuteNonQueryAsync();

    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }

    public string GetConnectionString()
    {
        return _msSqlContainer.GetConnectionString();
    }
}

[CollectionDefinition("SqlServer collection")]
public class SqlServerCollection : ICollectionFixture<SqlServerContainerFixture> { }
