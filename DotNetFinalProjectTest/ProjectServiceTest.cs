using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetFinalProject.Models;
using DotNetFinalProject.Services;
using Moq;
using Xunit;

namespace DotNetFinalProject
{
    public class ProjectServiceTest
    {
        [Fact]
        public async Task CreateProjectTest()
        {
            var fakeRepository = Mock.Of<IProjectRepository>();
            var projectService = new ProjectService(fakeRepository);

            var project = new Project() { Name = "test project" };
            await projectService.CreateProject(project);
        }
        
        [Fact]
        public async Task EditProjectTest()
        {
            var fakeRepository = Mock.Of<IProjectRepository>();
            var projectService = new ProjectService(fakeRepository);

            var project = new Project() { Name = "old name" };
            await projectService.CreateProject(project);
            project.Name = "name updated";
            await projectService.EditProject(project);
            
            Assert.Equal("name updated", project.Name);
          
        }
        
        [Fact]
        public async Task DeleteProjectTest()
        {
            var fakeRepository = Mock.Of<IProjectRepository>();
            var projectService = new ProjectService(fakeRepository);

            var project = new Project() { Id = 1, Name = "test" };
            await projectService.CreateProject(project);
            await projectService.DeleteProject(project);
            
            Assert.Null(await projectService.GetProject(1));
          
        }
        
        [Fact]
        public async Task GetProjectsTest()
        {
            var projects = new List<Project>
            {
                new Project() { Name = "test project 1" },
                new Project() { Name = "test project 2" },
            };

            var fakeRepositoryMock = new Mock<IProjectRepository>();
            fakeRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(projects);


            var projectService = new ProjectService(fakeRepositoryMock.Object);

            var resultProjects = await projectService.GetProjects();

            Assert.Collection(resultProjects, project =>
                {
                    Assert.Equal("test project 1", project.Name);
                },
                project =>
                {
                    Assert.Equal("test project 2", project.Name);
                });
        }
        
        [Fact]
        public async Task GetProjectTest()
        {
            var project =  new Project() { Id = 1, Name = "test project"};

            var fakeRepositoryMock = new Mock<IProjectRepository>();
            fakeRepositoryMock.Setup(x => x.GetOne(It.IsAny<long?>())).ReturnsAsync(project);
            
            var projectService = new ProjectService(fakeRepositoryMock.Object);

            var resultProject = await projectService.GetProject(1);
            
            Assert.Equal("test project", resultProject.Name);
        }
        
        [Fact]
        public async Task SpecialtyExistTest()
        {
            var fakeRepository = Mock.Of<IProjectRepository>();
            var projectService = new ProjectService(fakeRepository);

            var project = new Project() { Name = "test project" };
            await projectService.CreateProject(project);

            Assert.True(!projectService.ProjectExist(project.Id));
        }


    }
}