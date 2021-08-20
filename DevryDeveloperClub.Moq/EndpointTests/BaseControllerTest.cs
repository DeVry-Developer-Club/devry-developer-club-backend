using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevryDeveloperClub.Controllers;
using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Infrastructure.Data;
using DevryDeveloperClub.Infrastructure.Extensions;
using Moq;
using UnofficialDevryIT.Architecture.Models;
using Xunit;

namespace DevryDeveloperClub.Moq.EndpointTests
{
    /// <summary>
    /// Provides basic testing capabilities for all <see cref="ApiController{TEntity,TEntityDto}"/> entities
    /// </summary>
    /// <typeparam name="TController">Controller to test (must be of type <see cref="ApiController{TEntity,TEntityDto}"/>)</typeparam>
    /// <typeparam name="TEntity">The database object that is used</typeparam>
    /// <typeparam name="TEntityDto">The DTO object which represents the item passed between client and server (for creation)</typeparam>
    public abstract class BaseControllerTest<TController, TEntity, TEntityDto>
        where TController : ApiController<TEntity, TEntityDto>
        where TEntity : class, IEntityWithTypedId<string>, new()
        where TEntityDto : class, new()
    {
        // moq interfaces (system under test)
        protected readonly TController Sut;
        
        /// <summary>
        /// Mocked service that is required for <see cref="Sut"/> to work
        /// </summary>
        protected readonly Mock<IBaseDbService<TEntity>> ServiceMock = new();

        /// <summary>
        /// Data that "exists" in database
        /// </summary>
        protected readonly List<TEntity> SampleData;

        public BaseControllerTest()
        {
            SampleData = CreateSampleData();
            Sut = (TController)Activator.CreateInstance(typeof(TController), ServiceMock.Object);
        }

        #region Sample Info
        
        protected const string GuidOne = "a9fac008-00e5-11ec-9a03-0242ac130003";
        protected const string GuidTwo = "aeb8bef6-00e5-11ec-9a03-0242ac130003";
        protected const string ErrorFormat = "Could not locate item with Id: {0}";

        #endregion

        /// <summary>
        /// Generate sample data for the tests (requires at least 2 items)
        /// </summary>
        /// <returns></returns>
        protected abstract List<TEntity> CreateSampleData();

        /// <summary>
        /// As a user I am sending incomplete data for creating a new <typeparamref name="TEntity"/>
        /// Should respond with 400 error code with various error messages
        /// </summary>
        [Fact]
        public abstract Task CreateTag_WithInvalidData();

        /// <summary>
        /// As a user I want to create a new <typeparamref name="TEntity"/>
        /// Given valid data I should receive a 201 status code
        /// with the generated ID of the object
        /// </summary>
        [Fact]
        public async Task Create_ShouldReturnSuccessWithId()
        {
            var newData = SampleData.First().CloneTo<TEntityDto>();
            
            ResultOf<TEntity> result = new ResultOf<TEntity>()
            {
                Value = SampleData.First(),
                StatusCode = 201
            };
            
            ServiceMock.Setup(x => x.Create(It.IsAny<TEntity>()))
                .ReturnsAsync(result);
            
            // act
            var response = (dynamic)await Sut.Post(newData);
            
            // assert
            Assert.Equal(GuidOne, response.Value);
            Assert.Equal(201, response.StatusCode);
        }
        
        /// <summary>
        /// As a user I am searching for a <typeparamref name="TEntity"/> with a given ID
        /// It does not exist in the database
        /// Expecting a 404 -- not found error code
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenEntityNotExist()
        {
            // arrange
            ResultOf<TEntity> result = new ResultOf<TEntity>()
            {
                Value = null,
                StatusCode = 404,
                ErrorMessage = string.Format(ErrorFormat, GuidOne)
            };

            ServiceMock.Setup(x => x.Find(GuidOne))
                .ReturnsAsync(result);
            
            // act
            var response = (dynamic)await Sut.Get(GuidOne);
            
            // assert
            Assert.Equal(404, response.StatusCode);
            Assert.Equal(string.Format(ErrorFormat, GuidOne), response.Value);
        }
        
        /// <summary>
        /// As a user I am searching for a <typeparamref name="TEntity"/> with a given ID
        /// It does exist in the database
        /// Expecting a 200 status code with the <typeparamref name="TEntity"/> information as value
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
        {
            // arrange
            ResultOf<TEntity> result = new ResultOf<TEntity>()
            {
                Value = SampleData.First(),
                StatusCode = 200
            };

            ServiceMock.Setup(x => x.Find(GuidOne))
                .ReturnsAsync(result);
            
            // act
            var tag = (dynamic)await Sut.Get(GuidOne);
            
            // assert
            Assert.Equal(200, tag.StatusCode);
            Assert.Equal(SampleData.First().Id, tag.Value.Id);
        }
        
        /// <summary>
        /// As a user I want all the <typeparamref name="TEntity"/>s in the database
        /// 2 Tags exist in the database
        /// Expecting a list of 2 <typeparamref name="TEntity"/> to be returned
        /// </summary>
        [Fact]
        public async Task GetAll_ShouldReturnEntities_WhenEntitiesExist()
        {
            // arrange
            ServiceMock.Setup(x => x.Get())
                .ReturnsAsync(SampleData);
            
            // act
            var response = await Sut.Get();

            Assert.Equal(SampleData.Count, response.Count);
        }
        
        /// <summary>
        /// As a user I want to delete a <typeparamref name="TEntity"/> with a given ID
        /// The <typeparamref name="TEntity"/> exists in the database
        /// Expecting a 204 status code with no value
        /// </summary>
        [Fact]
        public async Task Delete_ShouldSucceed_WhenEntityExists()
        {
            // arrange
            ResultOf<TEntity> expect = new()
            {
                StatusCode = 204,
                Value = null,
                ErrorMessage = string.Empty
            };
            
            ServiceMock.Setup(x => x.Delete(GuidTwo))
                .ReturnsAsync(expect);

            var response = (dynamic) await Sut.Delete(GuidTwo);
            
            Assert.Equal(204, response.StatusCode);
        }
        
        /// <summary>
        /// As a user I want to delete a <typeparamref name="TEntity"/> with a given ID
        /// The <typeparamref name="TEntity"/> does NOT exist in the database
        /// Expecting a 404 -- not found status code
        /// </summary>
        [Fact]
        public async Task Delete_ShouldNotFind_WhenEntityDoesNotExist()
        {
            // arrange
            ResultOf<TEntity> expect = new()
            {
                StatusCode = 404,
                ErrorMessage = string.Format(ErrorFormat, GuidOne)
            };

            ServiceMock.Setup(x => x.Delete(GuidOne))
                .ReturnsAsync(expect);

            var response = (dynamic)await Sut.Delete(GuidOne);

            Assert.Equal(404, response.StatusCode);
            Assert.Equal(string.Format(ErrorFormat, GuidOne), response.Value);
        }
    }
}