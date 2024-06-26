using MediatRHandler.Data;
using MediatRHandler.Entities;
using Microsoft.EntityFrameworkCore;

public class CustomerRepository: ICustomerRepository
{
    private AppDbContext _dbContext;
    public CustomerRepository(AppDbContext context) 
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<Customer>> GetAll() {
        return await _dbContext.Customers.ToListAsync();
    }

    public async Task<Customer?> GetById(int id)
    {
        return await _dbContext.Customers.FindAsync(id);
    }

    public async Task<int> Create(Customer customer)
    {        
        await _dbContext.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
        return customer.Id;
    }

    public async Task Update(Customer customer) 
    {
        Customer? updateCustomer = await _dbContext.Customers.FindAsync(customer.Id);
        if (updateCustomer != null) {
            updateCustomer.FirstName = customer.FirstName;
            updateCustomer.LastName = customer.LastName;
            updateCustomer.EmailAddress = customer.EmailAddress;
            updateCustomer.Address = customer.Address;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async  Task Delete(int id) 
    {
        Customer? deleteCustomer = await _dbContext.Customers.FindAsync(id);
        if (deleteCustomer != null) {
            _dbContext.Customers.Remove(deleteCustomer);
            await _dbContext.SaveChangesAsync();
        }
    }
}