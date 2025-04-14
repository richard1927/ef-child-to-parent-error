using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace error_repro;


[Collection("SqlServer collection")]
public class ReproTest
{
    private readonly ReproContext _context;
    
    
    public ReproTest(SqlServerContainerFixture fixture)
    {
        var options = new DbContextOptionsBuilder<ReproContext>()
            .UseSqlServer(fixture.GetConnectionString())
            .Options;

        _context = new ReproContext(options);
    }

    
    [Fact]
    public async Task Test()
    {
        // Arrange
        var fromParent = new Parent{Id = 1, Name = "From"};
        _context.Parents.Add(fromParent);
        
        var toParent = new Parent{Id = 2, Name = "To"};
        _context.Parents.Add(toParent);
        
        var child = new Child{Id = 3, Name = "Child"};
        fromParent.Children.Add(child);
        _context.Children.Add(child);
        
        await _context.SaveChangesAsync();
        
        // Act
        fromParent.Children.Remove(child);
        toParent.Children.Add(child);
        
        await _context.SaveChangesAsync();
    }
}