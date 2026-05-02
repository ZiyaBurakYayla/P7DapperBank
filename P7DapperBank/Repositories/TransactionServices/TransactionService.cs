using Dapper;
using P7DapperBank.Context;
using P7DapperBank.Dtos.TransactionDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.TransactionServices
{
    public class TransactionService : ITransactionService
    {
        private readonly DapperContext _context;

        public TransactionService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateTransactionAsync(CreateTransactionDto createTransactionDto)
        {
            string query = @"Insert into transactions
                (account_number, transaction_date, amount, type_id, status, sender_bank_id, receiver_bank_id, description, currency, balance_after)
                Values
                (@AccountNumber, @TransactionDate, @Amount, @TypeId, @Status, @SenderBankId, @ReceiverBankId, @Description, @Currency, @BalanceAfter)";
            var parameters = new DynamicParameters();
            parameters.Add("AccountNumber", createTransactionDto.AccountNumber);
            parameters.Add("TransactionDate", createTransactionDto.TransactionDate);
            parameters.Add("Amount", createTransactionDto.Amount);
            parameters.Add("TypeId", createTransactionDto.TypeId);
            parameters.Add("Status", createTransactionDto.Status);
            parameters.Add("SenderBankId", createTransactionDto.SenderBankId);
            parameters.Add("ReceiverBankId", createTransactionDto.ReceiverBankId);
            parameters.Add("Description", createTransactionDto.Description);
            parameters.Add("Currency", createTransactionDto.Currency);
            parameters.Add("BalanceAfter", createTransactionDto.BalanceAfter);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            string query = "Delete from transactions where id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultTransactionDto>> GetAllTransactionAsync()
        {
            string query = "Select * from transactions";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultTransactionDto>(query);
            return values.ToList();
        }

        public async Task<GetByIdTransactionDto> GetTransactionByIdAsync(int id)
        {
            string query = "Select * from transactions where id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            var connection = _context.CreateConnection();
            var value = await connection.QueryFirstOrDefaultAsync<GetByIdTransactionDto>(query, parameters);
            return value;
        }

        public async Task UpdateTransactionAsync(UpdateTransactionDto updateTransactionDto)
        {
            string query = @"Update transactions set
                account_number = @AccountNumber, transaction_date = @TransactionDate, amount = @Amount,
                type_id = @TypeId, status = @Status, sender_bank_id = @SenderBankId,
                receiver_bank_id = @ReceiverBankId, description = @Description,
                currency = @Currency, balance_after = @BalanceAfter
                where id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", updateTransactionDto.Id);
            parameters.Add("AccountNumber", updateTransactionDto.AccountNumber);
            parameters.Add("TransactionDate", updateTransactionDto.TransactionDate);
            parameters.Add("Amount", updateTransactionDto.Amount);
            parameters.Add("TypeId", updateTransactionDto.TypeId);
            parameters.Add("Status", updateTransactionDto.Status);
            parameters.Add("SenderBankId", updateTransactionDto.SenderBankId);
            parameters.Add("ReceiverBankId", updateTransactionDto.ReceiverBankId);
            parameters.Add("Description", updateTransactionDto.Description);
            parameters.Add("Currency", updateTransactionDto.Currency);
            parameters.Add("BalanceAfter", updateTransactionDto.BalanceAfter);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<int> GetTransactionCountAsync()
        {
            string query = "Select Count(*) from transactions";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<decimal> GetTotalAmountAsync()
        {
            string query = "Select ISNULL(SUM(amount), 0) from transactions where status = 'SUCCESS'";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(query);
        }

        public async Task<List<TypeStatItem>> GetTypeStatsAsync()
        {
            string query = @"Select tt.type_name, Count(*) as Count
                from transactions t
                inner join transaction_types tt on t.type_id = tt.type_id
                group by tt.type_name
                order by Count(*) desc";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<TypeStatItem>(query);
            return values.ToList();
        }

        public async Task<List<MonthlyStatItem>> GetMonthlyStatsAsync()
        {
            string query = @"Select TOP 18
                YEAR(transaction_date) as Year,
                MONTH(transaction_date) as Month,
                Count(*) as Count,
                SUM(amount) as Total
                from transactions
                group by YEAR(transaction_date), MONTH(transaction_date)
                order by Year desc, Month desc";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<MonthlyStatItem>(query);
            return values.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();
        }

        public async Task<List<RecentTransactionItem>> GetRecentTransactionsAsync()
        {
            string query = @"Select TOP 10 t.id, t.account_number, t.transaction_date, t.amount, t.status, t.currency, tt.type_name
                from transactions t
                inner join transaction_types tt on t.type_id = tt.type_id
                order by t.transaction_date desc";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<RecentTransactionItem>(query);
            return values.ToList();
        }

        public async Task<PagedResult<ResultTransactionDto>> GetFilteredTransactionsAsync(FilterTransactionDto filter, int page, int pageSize)
        {
            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                whereClauses.Add("c.full_name LIKE '%' + @CustomerName + '%'");
                parameters.Add("CustomerName", filter.CustomerName);
            }
            if (!string.IsNullOrEmpty(filter.AccountNumber))
            {
                whereClauses.Add("t.account_number LIKE '%' + @AccountNumber + '%'");
                parameters.Add("AccountNumber", filter.AccountNumber);
            }
            if (!string.IsNullOrEmpty(filter.Status))
            {
                whereClauses.Add("t.status = @Status");
                parameters.Add("Status", filter.Status);
            }
            if (!string.IsNullOrEmpty(filter.Currency))
            {
                whereClauses.Add("t.currency = @Currency");
                parameters.Add("Currency", filter.Currency);
            }
            if (filter.TypeId.HasValue)
            {
                whereClauses.Add("t.type_id = @TypeId");
                parameters.Add("TypeId", filter.TypeId.Value);
            }
            if (filter.AmountMin.HasValue)
            {
                whereClauses.Add("t.amount >= @AmountMin");
                parameters.Add("AmountMin", filter.AmountMin.Value);
            }
            if (filter.AmountMax.HasValue)
            {
                whereClauses.Add("t.amount <= @AmountMax");
                parameters.Add("AmountMax", filter.AmountMax.Value);
            }

            string joins = @"from transactions t
                LEFT JOIN accounts a ON t.account_number = a.account_number
                LEFT JOIN customers c ON a.customer_id = c.customer_id";
            string where = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";
            int offset = (page - 1) * pageSize;
            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize);

            string countQuery = $"Select Count(*) {joins} {where}";
            string dataQuery = $@"Select t.*, c.full_name as customer_name
                {joins} {where}
                ORDER BY t.transaction_date DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var connection = _context.CreateConnection();
            int total = await connection.ExecuteScalarAsync<int>(countQuery, parameters);
            var items = await connection.QueryAsync<ResultTransactionDto>(dataQuery, parameters);

            return new PagedResult<ResultTransactionDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
