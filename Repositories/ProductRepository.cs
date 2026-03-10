using Dapper;
using InventoryApp.Data;
using InventoryApp.Models;

namespace InventoryApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = "SELECT * FROM Products ORDER BY Id DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Products WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(Product product)
        {
            var query = @"
                INSERT INTO Products (Name, Quantity, Price)
                VALUES (@Name, @Quantity, @Price);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, product);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var query = @"
                UPDATE Products 
                SET Name = @Name,
                    Quantity = @Quantity,
                    Price = @Price
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, product);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Products WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
