using System.Collections.Generic;
using System.Threading.Tasks;
using DevryDeveloperClub.Controllers;
using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Domain.Models;
using Moq;
using Xunit;

namespace DevryDeveloperClub.Moq.EndpointTests
{
    /// <summary>
    /// Test CRUD operations against <see cref="ProjectController"/>
    /// </summary>
    public class ProjectControllerTests : BaseControllerTest<ProjectController, Project, ProjectDto>
    {
        /// <inheritdoc cref="BaseControllerTest{TController,TEntity,TEntityDto}"/>
        protected override List<Project> CreateSampleData()
        {
            return new List<Project>
            {
                new()
                {
                    Id = GuidOne,
                    Name = "DeVry Developer Club Frontend",
                    TechStack = "Some awesome stuff",
                    GithubLink = "Https://github.com/DevryDeveloperClub/devry-developer-club-frontned"
                },

                new()
                {
                    Id = GuidTwo,
                    Name = "DeVry Developer Club Backend",
                    TechStack = "C# and cool stuff",
                    GithubLink = "I am a banana"
                }
            };
        }

        /// <inheritdoc cref="BaseControllerTest{TController,TEntity,TEntityDto}"/>
        public override async Task CreateTag_WithInvalidData()
        {
            ServiceMock.Setup(x => x.Create(It.IsAny<Project>()))
                .ReturnsAsync(new ResultOf<Project>());

            // Should get 3 error messages
            var allInvalid = await (dynamic)Sut.Post(new ProjectDto());

            Assert.Equal(400, allInvalid.StatusCode);
            Assert.Equal(3, allInvalid.Value.Length);
            
            // Should get 2 error messages
            var twoInvalid = await (dynamic)Sut.Post(new ProjectDto() { Name = "asdf" });
            Assert.Equal(400, twoInvalid.StatusCode);
            Assert.Equal(2, twoInvalid.Value.Length);
            
            // Should get 1 error message
            var oneInvalid = await (dynamic)Sut.Post(new ProjectDto() { Name = "asdf", GithubLink = "asdfasdfasd" });
            Assert.Equal(400, oneInvalid.StatusCode);
            Assert.Equal(1, oneInvalid.Value.Length);
        }
    }
}