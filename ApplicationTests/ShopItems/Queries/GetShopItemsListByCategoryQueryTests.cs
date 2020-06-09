﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Interfaces.Persistence;
using Application.ShopItems.Queries.GetShopItemsList;
using AutoFixture;
using AutoMapper;
using Domain.Categories;
using Domain.ShopItems;
using Moq;
using Xunit;

namespace Application.Tests.ShopItems.Queries
{
    public class GetShopItemsListByCategoryQueryTests
    {
        public GetShopItemsListByCategoryQueryTests()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new ShopItemProfile()); });
            _mapper = mapperConfiguration.CreateMapper();

            _fixture = new Fixture();

            // client has a circular reference from AutoFixture point of view
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _shopItems = _fixture.CreateMany<ShopItem>(5).ToList();

        }

        private readonly IMapper _mapper;
        private readonly List<ShopItem> _shopItems;
        private readonly Fixture _fixture;

        [Fact]
        public void TestExecuteShouldReturnListOfShopItemModelWithSpecifiedCategory()
        {
            //Arrange
            const int categoryId = 2;

            var shopItem = new ShopItem
                {Id = 2, Name = "testItemName2", CategoryId = 2, Category = new Category() {Id = 2}};

            _shopItems.Add(shopItem);


            var expectedResult = _mapper.Map<ShopItemModel>(shopItem);

                var mockShopItemRepository = new Mock<IShopItemRepository>();

            mockShopItemRepository.Setup(s => s.GetAll()).Returns(_shopItems.AsQueryable);

            var sut = new GetShopItemsListByCategoryQuery(mockShopItemRepository.Object, _mapper);


            //Act
            var result = sut.Execute(categoryId);

            //Assert
            Assert.IsType<List<ShopItemModel>>(result);

            Assert.Contains(expectedResult,result);
        }
    }
}
