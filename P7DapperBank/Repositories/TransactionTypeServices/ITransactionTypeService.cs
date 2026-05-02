using P7DapperBank.Dtos.TransactionTypeDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.TransactionTypeServices
{
    public interface ITransactionTypeService
    {
        Task<List<ResultTransactionTypeDto>> GetAllTransactionTypeAsync();
        Task<GetByIdTransactionTypeDto> GetTransactionTypeByIdAsync(int id);
        Task CreateTransactionTypeAsync(CreateTransactionTypeDto createTransactionTypeDto);
        Task UpdateTransactionTypeAsync(UpdateTransactionTypeDto updateTransactionTypeDto);
        Task DeleteTransactionTypeAsync(int id);

        Task<PagedResult<ResultTransactionTypeDto>> GetFilteredTransactionTypesAsync(FilterTransactionTypeDto filter, int page, int pageSize);
    }
}
