using Dapper;
using P7DapperBank.Context;
using P7DapperBank.Dtos.BankDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.BankServices
{
    public class BankService : IBankService
    {
        private readonly DapperContext _context;

        public BankService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateBankAsync(CreateBankDto createBankDto)
        {
            string query = "Insert into banks (bank_name) Values (@BankName)";
            var parameters = new DynamicParameters();
            parameters.Add("BankName", createBankDto.BankName);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteBankAsync(int id)
        {
            string query = "Delete from banks where bank_id = @BankId";
            var parameters = new DynamicParameters();
            parameters.Add("BankId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultBankDto>> GetAllBankAsync()
        {
            string query = "Select * from banks";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultBankDto>(query);
            return values.ToList();
        }

        public async Task<GetByIdBankDto> GetBankByIdAsync(int id)
        {
            string query = "Select * from banks where bank_id = @BankId";
            var parameters = new DynamicParameters();
            parameters.Add("BankId", id);
            var connection = _context.CreateConnection();
            var value = await connection.QueryFirstOrDefaultAsync<GetByIdBankDto>(query, parameters);
            return value;
        }

        public async Task UpdateBankAsync(UpdateBankDto updateBankDto)
        {
            string query = "Update banks set bank_name = @BankName where bank_id = @BankId";
            var parameters = new DynamicParameters();
            parameters.Add("BankId", updateBankDto.BankId);
            parameters.Add("BankName", updateBankDto.BankName);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<int> GetBankCountAsync()
        {
            string query = "Select Count(*) from banks";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<PagedResult<ResultBankDto>> GetFilteredBanksAsync(FilterBankDto filter, int page, int pageSize)
        {
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.BankName))
            {
                whereClauses.Add("bank_name LIKE '%' + @BankName + '%'");
                parameters.Add("BankName", filter.BankName);
            }

            string where = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";
            int offset = (page - 1) * pageSize;
            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            string countQuery = $"Select Count(*) from banks {where}";
            string dataQuery = $"Select * from banks {where} ORDER BY bank_id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var connection = _context.CreateConnection();
            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);
            var items = await connection.QueryAsync<ResultBankDto>(dataQuery, parameters);

            return new PagedResult<ResultBankDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
