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
    /// Tests CRUD operations against <see cref="TagController"/>
    /// </summary>
    public class TagControllerTests : BaseControllerTest<TagController, Tag, CreateTagDto>
    {
        /// <inheritdoc cref="BaseControllerTest{TController,TEntity,TEntityDto}"/>
        protected override List<Tag> CreateSampleData()
        {
            return new List<Tag>
            {
                new()
                {
                    Id = GuidOne,
                    Name = "Sample Name 1",
                    Color = "rgb(1,1,1)"
                },
                new()
                {
                    Id = GuidTwo,
                    Name = "Sample Name 2",
                    Color = "rgb(2,2,2)"
                }
            };
        }

        /// <inheritdoc cref="BaseControllerTest{TController,TEntity,TEntityDto}"/>
        public override async Task CreateTag_WithInvalidData()
        {
            ServiceMock.Setup(x => x.Create(It.IsAny<Tag>()))
                .ReturnsAsync(new ResultOf<Tag>());
            
            var allInvalid = await (dynamic)Sut.Post(new CreateTagDto());

            Assert.Equal(400, allInvalid.StatusCode);
            Assert.Equal(2, allInvalid.Value.Length);

            var nameInvalid = await (dynamic)Sut.Post(new CreateTagDto() { Color = "asdf" });
            Assert.Equal(400, nameInvalid.StatusCode);
            Assert.Equal(1, nameInvalid.Value.Length);

            var colorInvalid = await (dynamic)Sut.Post(new CreateTagDto() { Name = "asdf" });
            Assert.Equal(400, colorInvalid.StatusCode);
            Assert.Equal(1, colorInvalid.Value.Length);
        }
    }
}