using P7DapperBank.Dtos.AccountDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.AccountServices
{
    public interface IAccountService
    {
        Task<List<ResultAccountDto>> GetAllAccountAsync();
        Task<GetByIdAccountDto> GetAccountByAccountNumberAsync(string accountNumber);
        Task CreateAccountAsync(CreateAccountDto createAccountDto);
        Task UpdateAccountAsync(UpdateAccountDto updateAccountDto);
        Task DeleteAccountAsync(string accountNumber);

        Task<int> GetAccountCountAsync();
        Task<PagedResult<ResultAccountDto>> GetFilteredAccountsAsync(FilterAccountDto filter, int page, int pageSize);
    }
}
