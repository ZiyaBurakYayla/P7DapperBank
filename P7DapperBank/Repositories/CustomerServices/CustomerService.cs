using Dapper;
using P7DapperBank.Context;
using P7DapperBank.Dtos.CustomerDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly DapperContext _context;

        public CustomerService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            string query = "Insert into customers (full_name) Values (@FullName)";
            var parameters = new DynamicParameters();
            parameters.Add("FullName", createCustomerDto.FullName);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            string query = "Delete from customers where customer_id = @CustomerId";
            var parameters = new DynamicParameters();
            parameters.Add("CustomerId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultCustomerDto>> GetAllCustomerAsync()
        {
            string query = "Select * from customers";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultCustomerDto>(query);
            return values.ToList();
        }

        public async Task<GetByIdCustomerDto> GetCustomerByIdAsync(int id)
        {
            string query = "Select * from customers where customer_id = @CustomerId";
            var parameters = new DynamicParameters();
            parameters.Add("CustomerId", id);
            var connection = _context.CreateConnection();
            var value = await connection.QueryFirstOrDefaultAsync<GetByIdCustomerDto>(query, parameters);
            return value;
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            string query = "Update customers set full_name = @FullName where customer_id = @CustomerId";
            var parameters = new DynamicParameters();
            parameters.Add("FullName", updateCustomerDto.FullName);
            parameters.Add("CustomerId", updateCustomerDto.CustomerId);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<int> GetCustomerCountAsync()
        {
            string query = "Select Count(*) from customers";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<PagedResult<ResultCustomerDto>> GetFilteredCustomersAsync(FilterCustomerDto filter, int page, int pageSize)
        {
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.FullName))
            {
                whereClauses.Add("full_name LIKE '%' + @FullName + '%'");
                parameters.Add("FullName", filter.FullName);
            }

            string where = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";
            int offset = (page - 1) * pageSize;
            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            string countQuery = $"Select Count(*) from customers {where}";
            string dataQuery = $"Select * from customers {where} ORDER BY customer_id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var connection = _context.CreateConnection();
            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);
            var items = await connection.QueryAsync<ResultCustomerDto>(dataQuery, parameters);

            return new PagedResult<ResultCustomerDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
