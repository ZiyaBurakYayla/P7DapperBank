using Dapper;
using P7DapperBank.Context;
using P7DapperBank.Dtos.AccountDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly DapperContext _context;

        public AccountService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAccountAsync(CreateAccountDto createAccountDto)
        {
            string query = "Insert into accounts (account_number, customer_id) Values (@AccountNumber, @CustomerId)";
            var parameters = new DynamicParameters();
            parameters.Add("AccountNumber", createAccountDto.AccountNumber);
            parameters.Add("CustomerId", createAccountDto.CustomerId);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAccountAsync(string accountNumber)
        {
            string query = "Delete from accounts where account_number = @AccountNumber";
            var parameters = new DynamicParameters();
            parameters.Add("AccountNumber", accountNumber);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultAccountDto>> GetAllAccountAsync()
        {
            string query = "Select * from accounts";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultAccountDto>(query);
            return values.ToList();
        }

        public async Task<GetByIdAccountDto> GetAccountByAccountNumberAsync(string accountNumber)
        {
            string query = "Select * from accounts where account_number = @AccountNumber";
            var parameters = new DynamicParameters();
            parameters.Add("AccountNumber", accountNumber);
            var connection = _context.CreateConnection();
            var value = await connection.QueryFirstOrDefaultAsync<GetByIdAccountDto>(query, parameters);
            return value;
        }

        public async Task UpdateAccountAsync(UpdateAccountDto updateAccountDto)
        {
            string query = "Update accounts set customer_id = @CustomerId where account_number = @AccountNumber";
            var parameters = new DynamicParameters();
            parameters.Add("AccountNumber", updateAccountDto.AccountNumber);
            parameters.Add("CustomerId", updateAccountDto.CustomerId);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<int> GetAccountCountAsync()
        {
            string query = "Select Count(*) from accounts";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<PagedResult<ResultAccountDto>> GetFilteredAccountsAsync(FilterAccountDto filter, int page, int pageSize)
        {
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.AccountNumber))
            {
                whereClauses.Add("account_number LIKE '%' + @AccountNumber + '%'");
                parameters.Add("AccountNumber", filter.AccountNumber);
            }
            if (filter.CustomerId.HasValue)
            {
                whereClauses.Add("customer_id = @CustomerId");
                parameters.Add("CustomerId", filter.CustomerId.Value);
            }

            string where = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";
            int offset = (page - 1) * pageSize;
            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            string countQuery = $"Select Count(*) from accounts {where}";
            string dataQuery = $"Select * from accounts {where} ORDER BY account_number OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var connection = _context.CreateConnection();
            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);
            var items = await connection.QueryAsync<ResultAccountDto>(dataQuery, parameters);

            return new PagedResult<ResultAccountDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
