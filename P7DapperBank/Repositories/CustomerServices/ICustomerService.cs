using P7DapperBank.Dtos.CustomerDtos;
using P7DapperBank.Models;

namespace P7DapperBank.Repositories.CustomerServices
{
    public interface ICustomerService
    {
        Task<List<ResultCustomerDto>> GetAllCustomerAsync();
        Task<GetByIdCustomerDto> GetCustomerByIdAsync(int id);
        Task CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int id);

        Task<int> GetCustomerCountAsync();
        Task<PagedResult<ResultCustomerDto>> GetFilteredCustomersAsync(FilterCustomerDto filter, int page, int pageSize);
    }
}
