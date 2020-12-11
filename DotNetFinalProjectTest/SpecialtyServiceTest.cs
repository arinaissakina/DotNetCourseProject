using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetFinalProject.Models;
using DotNetFinalProject.Services;
using Moq;
using Xunit;

namespace DotNetFinalProject
{
    public class SpecialtyServiceTest
    {
        [Fact]
        public async Task CreateSpecialtyTest()
        {
            var fakeRepository = Mock.Of<ISpecialtyRepository>();
            var specialtyService = new SpecialtyService(fakeRepository);

            var specialty = new Specialty() { Name = "test specialty" };
            await specialtyService.CreateSpecialty(specialty);
        }
        
        [Fact]
        public async Task EditSpecialtyTest()
        {
            var fakeRepository = Mock.Of<ISpecialtyRepository>();
            var specialtyService = new SpecialtyService(fakeRepository);

            var specialty = new Specialty() { Name = "old name" };
            
            await specialtyService.CreateSpecialty(specialty);
            specialty.Name = "name updated";
            await specialtyService.EditSpecialty(specialty);
            
            Assert.Equal("name updated", specialty.Name);
          
        }
        
        [Fact]
        public async Task DeleteSpecialtyTest()
        {
            var fakeRepository = Mock.Of<ISpecialtyRepository>();
            var specialtyService = new SpecialtyService(fakeRepository);

            var specialty = new Specialty() { Id = 1, Name = "test"};
            
            await specialtyService.CreateSpecialty(specialty);
            await specialtyService.DeleteSpecialty(specialty);
            
            Assert.Null(await specialtyService.GetSpecialty(1));
          
        }
        
        [Fact]
        public async Task GetSpecialtiesTest()
        {
            var specialties = new List<Specialty>
            {
                new Specialty() { Name = "test specialty 1" },
                new Specialty() { Name = "test specialty 2" },
            };

            var fakeRepositoryMock = new Mock<ISpecialtyRepository>();
            fakeRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(specialties);


            var specialtyService = new SpecialtyService(fakeRepositoryMock.Object);

            var resultSpecialties = await specialtyService.GetSpecialties();

            Assert.Collection(resultSpecialties, specialty =>
                {
                    Assert.Equal("test specialty 1", specialty.Name);
                },
                specialty =>
                {
                    Assert.Equal("test specialty 2", specialty.Name);
                });
        }
        
        [Fact]
        public async Task GetSpecialtyTest()
        {
            var specialty =  new Specialty() { Id = 1, Name = "test specialty"};

            var fakeRepositoryMock = new Mock<ISpecialtyRepository>();
            fakeRepositoryMock.Setup(x => x.GetOne(It.IsAny<long?>())).ReturnsAsync(specialty);
            
            var specialtyService = new SpecialtyService(fakeRepositoryMock.Object);

            var resultSpecialty = await specialtyService.GetSpecialty(1);
            
            Assert.Equal("test specialty", resultSpecialty.Name);
        }

        [Fact]
        public async Task SpecialtyExistTest()
        {
            var fakeRepository = Mock.Of<ISpecialtyRepository>();
            var specialtyService = new SpecialtyService(fakeRepository);

            var specialty = new Specialty() { Id = 100, Name = "test" };
            
            await specialtyService.CreateSpecialty(specialty);

            Assert.True(!specialtyService.SpecialtyExist(specialty.Id));
        }
        
        
    }
}