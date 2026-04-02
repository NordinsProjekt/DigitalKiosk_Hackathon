using Entities;
using Services.Interfaces;

namespace FlowVisualizer.Core.Decorators;

public class TracingCustomerRepository(ICustomerRepository inner, FlowTracer tracer) : ICustomerRepository
{
    public Task<List<Customer>> GetAllAsync()
    {
        return tracer.TraceAsync(
            "CustomerService", "GetAllAsync",
            "CustomerRepository", "GetAllAsync",
            "Repository",
            () => inner.GetAllAsync(),
            payloadType: "Customer[]");
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        return tracer.TraceAsync(
            "CustomerService", "GetByIdAsync",
            "CustomerRepository", "GetByIdAsync",
            "Repository",
            () => inner.GetByIdAsync(id),
            input: new { id }, payloadType: "Customer");
    }

    public Task AddAsync(Customer customer)
    {
        return tracer.TraceAsync(
            "CustomerService", "AddAsync",
            "CustomerRepository", "AddAsync",
            "Repository",
            () => inner.AddAsync(customer),
            input: customer, payloadType: "Customer");
    }

    public Task UpdateNameAsync(Customer customer)
    {
        return tracer.TraceAsync(
            "CustomerService", "UpdateNameAsync",
            "CustomerRepository", "UpdateNameAsync",
            "Repository",
            () => inner.UpdateNameAsync(customer),
            input: customer, payloadType: "Customer");
    }

    public Task UpdateIdentityNumberAsync(Customer customer)
    {
        return tracer.TraceAsync(
            "CustomerService", "UpdateIdentityNumberAsync",
            "CustomerRepository", "UpdateIdentityNumberAsync",
            "Repository",
            () => inner.UpdateIdentityNumberAsync(customer),
            input: customer, payloadType: "Customer");
    }

    public Task DeleteAsync(Guid id)
    {
        return tracer.TraceAsync(
            "CustomerService", "DeleteAsync",
            "CustomerRepository", "DeleteAsync",
            "Repository",
            () => inner.DeleteAsync(id),
            input: new { id }, payloadType: "Guid");
    }

    public Task<Customer?> GetByPersonalIdentityNumberAsync(string personalIdentityNumber)
    {
        return tracer.TraceAsync(
            "CustomerService", "GetByPersonalIdentityNumberAsync",
            "CustomerRepository", "GetByPersonalIdentityNumberAsync",
            "Repository",
            () => inner.GetByPersonalIdentityNumberAsync(personalIdentityNumber),
            input: new { personalIdentityNumber }, payloadType: "Customer");
    }
}
