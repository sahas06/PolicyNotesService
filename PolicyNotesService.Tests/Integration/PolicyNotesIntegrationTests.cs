using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PolicyNotesService.Data;
using PolicyNotesService.Dtos;
using Xunit;

namespace PolicyNotesService.Tests.Integration;

public class PolicyNotesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PolicyNotesIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove ALL registrations of AppDbContext and options
                var contexts = services
                    .Where(d => d.ServiceType == typeof(AppDbContext) ||
                                d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                    .ToList();

                foreach (var c in contexts)
                    services.Remove(c);

                // Register NEW InMemory DB that overrides API DB completely
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTestDB");
                });
            });
        });
    }

    [Fact]
    public async Task PostNotes_Returns_201Created()
    {
        var client = _factory.CreateClient();

        var create = new CreatePolicyNoteDto("POL-POST-1", "test note");
        var response = await client.PostAsJsonAsync("/notes", create);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetNotes_Returns_200Ok()
    {
        var client = _factory.CreateClient();

        await client.PostAsJsonAsync("/notes", new CreatePolicyNoteDto("POL-GET-1", "note"));

        var response = await client.GetAsync("/notes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var list = await response.Content.ReadFromJsonAsync<List<PolicyNoteDto>>();
        Assert.True(list!.Count > 0);
    }

    [Fact]
    public async Task GetById_Returns_200_Or_404()
    {
        var client = _factory.CreateClient();

        var create = new CreatePolicyNoteDto("POL-ID-1", "id-test");
        var postResult = await client.PostAsJsonAsync("/notes", create);
        var created = await postResult.Content.ReadFromJsonAsync<PolicyNoteDto>();

        var goodResponse = await client.GetAsync($"/notes/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, goodResponse.StatusCode);

        var missing = await client.GetAsync("/notes/99999");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }
}
