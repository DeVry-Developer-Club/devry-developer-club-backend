using System.Collections.Generic;
using System.Threading.Tasks;
using DevryDeveloperClub.Controllers;
using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Data;
using Moq;
using Xunit;

namespace DevryDeveloperClub.Moq.EndpointTests
{
    public class TagControllerTests
    {
        // moq interfaces (system under test)
        private readonly TagController _sut;
        
        /// <summary>
        /// Mocked service that is required for <see cref="_sut"/> to work
        /// </summary>
        private readonly Mock<IBaseDbService<Tag>> _serviceMock = new();
        
        #region Test Data
        
        const string TagOneId = "a9fac008-00e5-11ec-9a03-0242ac130003";
        const string TagTwoId = "aeb8bef6-00e5-11ec-9a03-0242ac130003";
        private const string ErrorFormat = "Could not locate item with Id: {0}";
        
        private readonly Tag _tagOne = new()
        {
            Id = TagOneId,
            Name = "Sample Tag",
            ColorValue = "rgb(0,0,0)"
        };

        private readonly Tag _tagTwo = new()
        {
            Id = TagTwoId,
            Name = "Sample 2 Tag",
            ColorValue = "rgb(1,1,1)"
        };
        
        #endregion
        
        public TagControllerTests()
        {
            _sut = new TagController(_serviceMock.Object);
        }

        /// <summary>
        /// As a user I am sending incomplete data for creating a new tag
        /// Should respond with 400 error code and 'Invalid Data' error message
        /// </summary>
        [Fact]
        public async Task CreateTag_WithInvalidData()
        {
            var allInvalid = await (dynamic)_sut.Post(new CreateTagDto());

            Assert.Equal(400, allInvalid.StatusCode);
            Assert.Equal("Invalid Data", allInvalid.Value);

            var nameInvalid = await (dynamic)_sut.Post(new CreateTagDto() { Color = "asdf" });
            Assert.Equal(400, nameInvalid.StatusCode);
            Assert.Equal("Invalid Data", nameInvalid.Value);

            var colorInvalid = await (dynamic)_sut.Post(new CreateTagDto() { Name = "asdf" });
            Assert.Equal(400, colorInvalid.StatusCode);
            Assert.Equal("Invalid Data", colorInvalid.Value);
        }
        
        /// <summary>
        /// As a user I want to create a new tag
        /// Given valid data I should receive a 201 status code
        /// with the generated ID of the object
        /// </summary>
        [Fact]
        public async Task CreateTag_ShouldReturnSuccessWithId()
        {
            // arrange
            CreateTagDto newData = new()
            {
                Color = _tagOne.Name,
                Name = _tagOne.ColorValue
            };
            
            ResultOf<Tag> result = new ResultOf<Tag>()
            {
                Value = _tagOne,
                StatusCode = 201
            };
            
            _serviceMock.Setup(x => x.Create(It.IsAny<Tag>()))
                .ReturnsAsync(result);
            
            // act
            var response = (dynamic)await _sut.Post(newData);
            
            // assert
            Assert.Equal(TagOneId, response.Value);
            Assert.Equal(201, response.StatusCode);
        }

        /// <summary>
        /// As a user I am searching for a tag with a given ID
        /// It does not exist in the database
        /// Expecting a 404 -- not found error code
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenTagNotExist()
        {
            // arrange
            ResultOf<Tag> result = new ResultOf<Tag>()
            {
                Value = null,
                StatusCode = 404,
                ErrorMessage = string.Format(ErrorFormat, TagOneId)
            };

            _serviceMock.Setup(x => x.Find(TagOneId))
                .ReturnsAsync(result);
            
            // act
            var response = (dynamic)await _sut.Get(TagOneId);
            
            // assert
            Assert.Equal(404, response.StatusCode);
            Assert.Equal(string.Format(ErrorFormat, TagOneId), response.Value);
        }
        
        /// <summary>
        /// As a user I am searching for a tag with a given ID
        /// It does exist in the database
        /// Expecting a 200 status code with the Tag information as value
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnTag_WhenTagExists()
        {
            // arrange
            ResultOf<Tag> result = new ResultOf<Tag>()
            {
                Value = _tagOne,
                StatusCode = 200
            };

            _serviceMock.Setup(x => x.Find(TagOneId))
                .ReturnsAsync(result);
            
            // act
            var tag = (dynamic)await _sut.Get(TagOneId);
            
            // assert
            Assert.Equal(200, tag.StatusCode);
            Assert.Equal(_tagOne.Id, tag.Value.Id);
        }

        /// <summary>
        /// As a user I want all the tags in the database
        /// 2 Tags exist in the database
        /// Expecting a list of 2 tags to be returned
        /// </summary>
        [Fact]
        public async Task GetAll_ShouldReturnTags_WhenTagsExist()
        {
            // arrage
            List<Tag> result = new() { _tagOne, _tagTwo };
            _serviceMock.Setup(x => x.Get())
                .ReturnsAsync(result);
            
            // act
            var response = await _sut.Get();

            Assert.Equal(2, response.Count);
        }

        /// <summary>
        /// As a user I want to delete a tag with a given ID
        /// The tag exists in the database
        /// Expecting a 204 status code with no value
        /// </summary>
        [Fact]
        public async Task Delete_ShouldSucceed_WhenTagExists()
        {
            // arrange
            ResultOf<Tag> expect = new()
            {
                StatusCode = 204,
                Value = null,
                ErrorMessage = string.Empty
            };
            
            _serviceMock.Setup(x => x.Delete(TagTwoId))
                .ReturnsAsync(expect);

            var response = (dynamic) await _sut.Delete(TagTwoId);
            
            Assert.Equal(204, response.StatusCode);
        }

        /// <summary>
        /// As a user I want to delete a tag with a given ID
        /// The tag does NOT exist in the database
        /// Expecting a 404 -- not found status code
        /// </summary>
        [Fact]
        public async Task Delete_ShouldNotFind_WhenTagDoesNotExist()
        {
            // arrange
            ResultOf<Tag> expect = new()
            {
                StatusCode = 404,
                ErrorMessage = string.Format(ErrorFormat, TagOneId)
            };

            _serviceMock.Setup(x => x.Delete(TagOneId))
                .ReturnsAsync(expect);

            var response = (dynamic)await _sut.Delete(TagOneId);

            Assert.Equal(404, response.StatusCode);
            Assert.Equal(string.Format(ErrorFormat, TagOneId), response.Value);
        }
        
    }
}