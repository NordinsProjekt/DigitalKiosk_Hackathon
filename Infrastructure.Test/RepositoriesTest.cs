using EF_MSSQL;
using EF_MSSQL.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Test;

public class RepositoriesTest
{
    private static readonly KioskDbContext context;

    [Fact]
    public static async Task CustomerRepository_FilteringProducts_IfItReturnsContainingWord()
    {

        var repository = new ProductRepository(context);

        var result = await repository.FilteringProducts("potatis");

        Assert.Contains(result, x => x.Name.Contains("pot"));
    }
}