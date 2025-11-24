using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Repositories;
using PolicyNotesService.Services;
using PolicyNotesService.Dtos;
using Xunit;

namespace PolicyNotesService.Tests.Unit;

public class PolicyNoteServiceUnitTests
{
    private AppDbContext CreateDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddNote_Should_Add_And_Return_Dto()
    {
        using var db = CreateDbContext(nameof(AddNote_Should_Add_And_Return_Dto));
        var repo = new PolicyNoteRepository(db);
        var service = new PolicyNoteService(repo);

        var dto = new CreatePolicyNoteDto("POL123", "First note");
        var result = await service.AddNoteAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("POL123", result.PolicyNumber);
        Assert.Equal("First note", result.Note);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task GetAll_Should_Return_List()
    {
        using var db = CreateDbContext(nameof(GetAll_Should_Return_List));

        db.PolicyNotes.Add(new PolicyNotesService.Models.PolicyNote { PolicyNumber = "P1", Note = "n1" });
        db.PolicyNotes.Add(new PolicyNotesService.Models.PolicyNote { PolicyNumber = "P2", Note = "n2" });
        await db.SaveChangesAsync();

        var repo = new PolicyNoteRepository(db);
        var service = new PolicyNoteService(repo);

        var list = await service.GetAllAsync();

        Assert.Equal(2, list.Count);
    }
}
