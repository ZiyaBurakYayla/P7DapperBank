using P7DapperBank.Dtos.BankDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.BankServices
{
    public interface IBankService
    {
        Task<List<ResultBankDto>> GetAllBankAsync();
        Task<GetByIdBankDto> GetBankByIdAsync(int id);
        Task CreateBankAsync(CreateBankDto createBankDto);
        Task UpdateBankAsync(UpdateBankDto updateBankDto);
        Task DeleteBankAsync(int id);

        Task<int> GetBankCountAsync();
        Task<PagedResult<ResultBankDto>> GetFilteredBanksAsync(FilterBankDto filter, int page, int pageSize);
    }
}
