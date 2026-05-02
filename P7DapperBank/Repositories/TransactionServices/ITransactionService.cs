using P7DapperBank.Dtos.TransactionDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.TransactionServices
{
    public interface ITransactionService
    {
        Task<List<ResultTransactionDto>> GetAllTransactionAsync();
        Task<GetByIdTransactionDto> GetTransactionByIdAsync(int id);
        Task CreateTransactionAsync(CreateTransactionDto createTransactionDto);
        Task UpdateTransactionAsync(UpdateTransactionDto updateTransactionDto);
        Task DeleteTransactionAsync(int id);

        Task<int> GetTransactionCountAsync();
        Task<decimal> GetTotalAmountAsync();
        Task<List<TypeStatItem>> GetTypeStatsAsync();
        Task<List<MonthlyStatItem>> GetMonthlyStatsAsync();
        Task<List<RecentTransactionItem>> GetRecentTransactionsAsync();
        Task<PagedResult<ResultTransactionDto>> GetFilteredTransactionsAsync(FilterTransactionDto filter, int page, int pageSize);
    }
}
