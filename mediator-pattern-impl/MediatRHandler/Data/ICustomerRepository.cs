using MediatRHandler.Entities;

namespace MediatRHandler.Data;
public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAll();
    Task<Customer?> GetById(int id);    
    Task<int> Create(Customer customer);
    Task Update(Customer customer);
    Task Delete(int id);
}