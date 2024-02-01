using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neptunee.Handlers.RequestDispatcher;
using Sample.Infrastructure.Persistence.Context;
using Sample.Infrastructure.Persistence.DataSeed;

namespace Sample.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public sealed class ZTestController : ControllerBase
{
    private readonly SampleDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public ZTestController(INeptuneeRequestDispatcher dispatcher, SampleDbContext context, IServiceProvider serviceProvider) 
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteDb(bool deleteIfNotDev = false)
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.MigrateAsync();
        await DataSeed.Seed(_context, _serviceProvider);
        return Ok("Done");
    }
    
    [HttpGet]
    public IActionResult TestThrowException()
    {
        throw new ApplicationException("This test");
    }
}