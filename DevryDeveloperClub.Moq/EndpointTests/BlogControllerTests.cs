using System;
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
    /// Tests CRUD operations against <see cref="BlogController"/>
    /// </summary>
    public class BlogControllerTests : BaseControllerTest<BlogController, Blog, BlogDTO>
    {
        /// <inheritdoc cref="BaseControllerTest{TController,TEntity,TEntityDto}"/>
        protected override List<Blog> CreateSampleData()
        {
            return new List<Blog>
            {
                new()
                {
                    Id = GuidOne,
                    Category = "Sample Category",
                    CreatedAt = DateTime.UtcNow
                },
                
                new()
                {
                    Id = GuidTwo,
                    Category = "Second Category",
                    CreatedAt = DateTime.UtcNow
                }
            };
        }

        /// <inheritdoc cref="BaseControllerTest{TController,TEntity,TEntityDto}"/>
        public override async Task CreateTag_WithInvalidData()
        {
            ServiceMock.Setup(x => x.Create(It.IsAny<Blog>()))
                .ReturnsAsync(new ResultOf<Blog>());
            
            var allInvalid = await (dynamic)Sut.Post(new BlogDTO());

            Assert.Equal(400, allInvalid.StatusCode);
            Assert.Equal(1, allInvalid.Value.Length);
        }
    }
}