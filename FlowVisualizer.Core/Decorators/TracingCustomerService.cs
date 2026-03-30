using Entities;
using Services.Interfaces;

namespace FlowVisualizer.Core.Decorators;

public class TracingCustomerService(ICustomerService inner, FlowTracer tracer) : ICustomerService
{
    public Task<List<Customer>> GetAllAsync()
    {
        return tracer.TraceAsync(
            "CustomersController", "GetAll",
            "CustomerService", "GetAllAsync",
            "Service",
            () => inner.GetAllAsync());
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        return tracer.TraceAsync(
            "CustomersController", "GetById",
            "CustomerService", "GetByIdAsync",
            "Service",
            () => inner.GetByIdAsync(id));
    }

    public Task AddAsync(Customer customer)
    {
        return tracer.TraceAsync(
            "CustomersController", "Add",
            "CustomerService", "AddAsync",
            "Service",
            () => inner.AddAsync(customer));
    }

    public Task DeleteAsync(Guid id)
    {
        return tracer.TraceAsync(
            "CustomersController", "Delete",
            "CustomerService", "DeleteAsync",
            "Service",
            () => inner.DeleteAsync(id));
    }

    public Task UpdateNameAsync(Customer customer)
    {
        return tracer.TraceAsync(
            "CustomersController", "UpdateName",
            "CustomerService", "UpdateNameAsync",
            "Service",
            () => inner.UpdateNameAsync(customer));
    }

    public Task UpdateIdentityNumberAsync(Customer customer)
    {
        return tracer.TraceAsync(
            "CustomersController", "UpdateIdentityNumber",
            "CustomerService", "UpdateIdentityNumberAsync",
            "Service",
            () => inner.UpdateIdentityNumberAsync(customer));
    }

    public Task<Customer?> GetByPersonalIdentityNumberAsync(string personalIdentityNumber)
    {
        return tracer.TraceAsync(
            "CustomersController", "GetByPIN",
            "CustomerService", "GetByPersonalIdentityNumberAsync",
            "Service",
            () => inner.GetByPersonalIdentityNumberAsync(personalIdentityNumber));
    }
}
