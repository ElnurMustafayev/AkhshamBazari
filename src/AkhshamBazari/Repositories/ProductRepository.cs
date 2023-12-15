namespace AkhshamBazari.Repositories;

using System.Data.SqlClient;
using AkhshamBazari.Models;
using AkhshamBazari.Repositories.Base;
using Dapper;

public class ProductRepository : IProductRepository
{
    private const string ConnectionString = "Server=localhost;Database=AkhshamBazariDb;User Id=sa;Password=Admin9264!!;";

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = new SqlConnection(ConnectionString);

        var products = await connection.QueryAsync<Product>("select * from Products");

        return products;
    }

    public async Task InsertProductAsync(Product product) {
        using var connection = new SqlConnection(ConnectionString);
        
        var products = await connection.ExecuteAsync(
            sql: "insert into Products (Name, Price) values (@Name, @Price);",
            param: product);
    }
}