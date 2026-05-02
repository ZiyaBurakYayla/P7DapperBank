using Dapper;
using P7DapperBank.Context;
using P7DapperBank.Dtos.TransactionTypeDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.TransactionTypeServices
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly DapperContext _context;

        public TransactionTypeService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateTransactionTypeAsync(CreateTransactionTypeDto createTransactionTypeDto)
        {
            string query = "Insert into transaction_types (type_name) Values (@TypeName)";
            var parameters = new DynamicParameters();
            parameters.Add("TypeName", createTransactionTypeDto.TypeName);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteTransactionTypeAsync(int id)
        {
            string query = "Delete from transaction_types where type_id = @TypeId";
            var parameters = new DynamicParameters();
            parameters.Add("TypeId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultTransactionTypeDto>> GetAllTransactionTypeAsync()
        {
            string query = "Select * from transaction_types";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultTransactionTypeDto>(query);
            return values.ToList();
        }

        public async Task<GetByIdTransactionTypeDto> GetTransactionTypeByIdAsync(int id)
        {
            string query = "Select * from transaction_types where type_id = @TypeId";
            var parameters = new DynamicParameters();
            parameters.Add("TypeId", id);
            var connection = _context.CreateConnection();
            var value = await connection.QueryFirstOrDefaultAsync<GetByIdTransactionTypeDto>(query, parameters);
            return value;
        }

        public async Task UpdateTransactionTypeAsync(UpdateTransactionTypeDto updateTransactionTypeDto)
        {
            string query = "Update transaction_types set type_name = @TypeName where type_id = @TypeId";
            var parameters = new DynamicParameters();
            parameters.Add("TypeId", updateTransactionTypeDto.TypeId);
            parameters.Add("TypeName", updateTransactionTypeDto.TypeName);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<PagedResult<ResultTransactionTypeDto>> GetFilteredTransactionTypesAsync(FilterTransactionTypeDto filter, int page, int pageSize)
        {
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.TypeName))
            {
                whereClauses.Add("type_name LIKE '%' + @TypeName + '%'");
                parameters.Add("TypeName", filter.TypeName);
            }

            string where = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";
            int offset = (page - 1) * pageSize;
            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            string countQuery = $"Select Count(*) from transaction_types {where}";
            string dataQuery = $"Select * from transaction_types {where} ORDER BY type_id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var connection = _context.CreateConnection();
            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);
            var items = await connection.QueryAsync<ResultTransactionTypeDto>(dataQuery, parameters);

            return new PagedResult<ResultTransactionTypeDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
