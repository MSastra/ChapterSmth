using Microsoft.Data.SqlClient;

public class AsyncSqlIsolationExamples
{
	private const string ConnectionString = "Data Source=.;Initial Catalog=YourDatabase;Integrated Security=True;TrustServerCertificate=True;";

	public static async Task SetupDatabase()
	{
		Console.WriteLine("Setting up database...");
		try
		{
			await using var connection = new SqlConnection(ConnectionString);
			await connection.OpenAsync();
			string createTableSql = @"
                IF OBJECT_ID('Products', 'U') IS NOT NULL DROP TABLE Products;
                CREATE TABLE Products (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(100),
                    Price DECIMAL(10, 2),
                    Stock INT
                );
                INSERT INTO Products (Name, Price, Stock) VALUES ('Laptop', 1200.00, 50);
                INSERT INTO Products (Name, Price, Stock) VALUES ('Mouse', 25.00, 200);
            ";
			await using var command = new SqlCommand(createTableSql, connection);
			await command.ExecuteNonQueryAsync();
			Console.WriteLine("Database setup complete: 'Products' table created and populated.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error during database setup: {ex.Message}");
		}
	}

	public static async Task ReadAllProducts(string caller = "Main")
	{
		await using var connection = new SqlConnection(ConnectionString);
		await connection.OpenAsync();
		await using var command = new SqlCommand("SELECT Id, Name, Price, Stock FROM Products ORDER BY Id", connection);
		Console.WriteLine($"\n--- {caller} Reading All Products ---");
		await using var reader = await command.ExecuteReaderAsync();
		if (!reader.HasRows)
		{
			Console.WriteLine("No products found.");
		}
		while (await reader.ReadAsync())
		{
			Console.WriteLine($"  Id: {reader["Id"]}, Name: {reader["Name"]}, Price: {reader["Price"]}, Stock: {reader["Stock"]}");
		}
		Console.WriteLine("-----------------------------------");
	}
}