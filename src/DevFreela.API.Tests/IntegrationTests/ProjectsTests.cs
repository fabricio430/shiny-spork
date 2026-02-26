using System.Net;
using System.Net.Http.Json;
using DevFreela.API.Database;
using DevFreela.API.Database.Entities;
using DevFreela.API.Database.Enums;
using DevFreela.API.Models.Projects;
using Microsoft.Extensions.DependencyInjection;

namespace DevFreela.API.Tests.IntegrationTests;

[TestFixture]
public class ProjectsTests
{
    private HttpClient _client = null!;
    private CustomWebApplicationFactory _factory = null!;
    private Guid _clientId;
    private Guid _freelancerId;
    private Guid _skillId;

    [SetUp]
    public async Task SetUp()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var client = new User
        {
            Name = "John Client",
            Email = "client@test.com",
            Password = "password123",
            Type = ETypeUser.Client,
            ProfilePicture = []
        };
        context.Users.Add(client);
        _clientId = client.Id;

        var freelancer = new User
        {
            Name = "Jane Freelancer",
            Email = "freelancer@test.com",
            Password = "password123",
            Type = ETypeUser.Freelancer,
            ProfilePicture = []
        };
        context.Users.Add(freelancer);
        _freelancerId = freelancer.Id;

        var skill = new Skill { Name = "C#" };
        context.Skills.Add(skill);
        _skillId = skill.Id;

        await context.SaveChangesAsync();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task DeveCriarProjetoComSucesso()
    {
        var createModel = new CreateProjectInputModel
        {
            Title = "Test Project",
            Description = "Test Description",
            TotalCost = 5000,
            ClientId = _clientId,
            FreelancerId = _freelancerId,
            RequiredSkilsIds = [_skillId]
        };

        var response = await _client.PostAsJsonAsync("/api/projects", createModel);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(response.Headers.Location, Is.Not.Null);
    }

    [Test]
    public async Task DeveObterProjetoPorIdComSucesso()
    {
        var projectId = await CreateTestProject();

        var response = await _client.GetAsync($"/api/projects/{projectId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var project = await response.Content.ReadFromJsonAsync<ProjectViewModel>();
        Assert.That(project, Is.Not.Null);
        Assert.That(project!.Id, Is.EqualTo(projectId));
        Assert.That(project.Title, Is.EqualTo("Test Project"));
    }

    [Test]
    public async Task DeveBuscarProjetosComSucesso()
    {
        await CreateTestProject("CRM System", "CRM for sales");
        await CreateTestProject("Blog Platform", "Personal blog");

        var response = await _client.GetAsync("/api/projects?search=CRM");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var projects = await response.Content.ReadFromJsonAsync<List<ProjectViewModel>>();
        Assert.That(projects, Is.Not.Null);
        Assert.That(projects!.Count, Is.GreaterThanOrEqualTo(1));
        Assert.That(projects.Any(p => p.Title.Contains("CRM")), Is.True);
    }

    [Test]
    public async Task DeveAtualizarProjetoComSucesso()
    {
        var projectId = await CreateTestProject();

        var updateModel = new UpdateProjectInputModel
        {
            Id = projectId,
            Title = "Updated Title",
            Description = "Updated Description",
            TotalCost = 7000
        };

        var response = await _client.PutAsJsonAsync($"/api/projects/{projectId}", updateModel);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeveIniciarProjetoComSucesso()
    {
        var projectId = await CreateTestProject();

        var response = await _client.PutAsync($"/api/projects/{projectId}/start", null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeveCompletarProjetoComSucesso()
    {
        var projectId = await CreateTestProject();

        var response = await _client.PutAsync($"/api/projects/{projectId}/complete", null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeveDesabilitarProjetoComSucesso()
    {
        var projectId = await CreateTestProject();

        var response = await _client.PostAsync($"/api/projects/{projectId}/disable", null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeveDeletarProjetoComSucesso()
    {
        var projectId = await CreateTestProject();

        var response = await _client.DeleteAsync($"/api/projects/{projectId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeveAdicionarComentarioAoProjetoComSucesso()
    {
        var projectId = await CreateTestProject();

        var commentModel = new CreateProjectCommentInputModel
        {
            Content = "Great project!",
            UserId = _clientId,
            ProjectId = projectId
        };

        var response = await _client.PostAsJsonAsync($"/api/projects/{projectId}/comments", commentModel);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task<Guid> CreateTestProject(string title = "Test Project", string description = "Test Description")
    {
        var createModel = new CreateProjectInputModel
        {
            Title = title,
            Description = description,
            TotalCost = 5000,
            ClientId = _clientId,
            FreelancerId = _freelancerId,
            RequiredSkilsIds = [_skillId]
        };

        var response = await _client.PostAsJsonAsync("/api/projects", createModel);
        response.EnsureSuccessStatusCode();

        var locationHeader = response.Headers.Location?.ToString();
        var idString = locationHeader?.Split('/').Last();

        return Guid.Parse(idString!);
    }
}
