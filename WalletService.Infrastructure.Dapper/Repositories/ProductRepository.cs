using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WalletService.Contracts.Persistence;
using WalletService.Domain.Entities;
using WalletService.Infrastructure.Dapper.Persistence;

namespace WalletService.Infrastructure.Dapper.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbSession _session;
        public ProductRepository(DbSession session)
        {
            _session = session;
        }

        public async Task DeleteAsync(Product entity)
        {
            var exec = "DELETE * FROM Products WHERE Id = @Id";
            await _session.Connection.ExecuteAsync(exec);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = "SELECT * FROM Products";
            return await _session.Connection.QueryAsync<Product>(query);
        }

        public async Task AddAsync(Product entity)
        {
            var exec = "INSERT INTO Products (Name, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate) VALUES (@Name, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate)";

            var parameters = new DynamicParameters();

            parameters.Add("Name", entity.Name, DbType.String);
            parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
            parameters.Add("CreatedDate", DateTime.Now.ToString(), DbType.String);
            parameters.Add("LastModifiedBy", entity.LastModifiedBy, DbType.String);
            parameters.Add("LastModifiedDate", entity.LastModifiedDate, DbType.String);

            await _session.Connection.ExecuteAsync(exec, parameters, _session.Transaction);
        }

        public Task<IReadOnlyList<Product>> GetAsync(Expression<Func<Product, bool>> predicate = null, Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetAsync(Expression<Func<Product, bool>> predicate = null, Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null, List<Expression<Func<Product, object>>> includes = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<Product> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Products WHERE Id = @Id";
            return await _session.Connection.QuerySingleOrDefaultAsync<Product>(query, new { id });
        }

        public async Task UpdateAsync(Product entity)
        {
            var exec = "UPDATE Products SET Name = @Name, CreatedBy = '', CreatedDate = @CreatedDate, LastModifiedBy = '', LastModifiedDate = '' WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", entity.Id, DbType.Int32);
            parameters.Add("Name", entity.Name, DbType.String);
            parameters.Add("CreatedBy", entity.CreatedBy, DbType.String);
            parameters.Add("CreatedDate", DateTime.Now.ToString(), DbType.String);
            parameters.Add("LastModifiedBy", entity.LastModifiedBy, DbType.String);
            parameters.Add("LastModifiedDate", entity.LastModifiedDate, DbType.String);

            await _session.Connection.ExecuteAsync(exec, parameters, _session.Transaction);
        }

        public Task<IReadOnlyList<Product>> GetAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
