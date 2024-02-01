using Neptunee.EntityFrameworkCore.Repository;
using Sample.Application.Core.Abstractions.Data;
using Sample.Domain.Repositories;

namespace Sample.Infrastructure.Persistence.Repositories;

public class Repository : NeptuneeRepository<ISampleDbContext>, IRepository
{
    public Repository(ISampleDbContext context) : base(context)
    {
    }
}