﻿using System;
using System.Collections.Generic;
using System.Linq;
using Application.Interfaces.Persistence;
using Domain.ShopItems;

namespace Application.ShopItems.Queries.GetShopItemsList
{
    public class GetShopItemsListQuery : IGetShopItemsListQuery
    {
        private readonly IShopItemRepository _shopItemRepository;

        public GetShopItemsListQuery(IShopItemRepository shopItemRepository)
        {
            _shopItemRepository = shopItemRepository;
        }
        public List<ShopItemsModel> Execute()
        {
            var shopItems =  _shopItemRepository.GetAll();
            var shopItemsModels = shopItems.Select(s=> ShopItemModelMapping(s));
            return shopItemsModels.ToList();
        }

       

        public ShopItemsModel Execute(int itemId)
        {
            var shopItem =_shopItemRepository.Get(itemId);
            return ShopItemModelMapping(shopItem);
        }


        private static ShopItemsModel ShopItemModelMapping(ShopItem shopItem)
        {
            //var shopItemsModels = shopItems.Select(s => new ShopItemsModel()
            var shopItemModel = new ShopItemsModel()
            {
                Name = shopItem.Name,
                ShortDescription = shopItem.ShortDescription,
                LongDescription = shopItem.LongDescription,
                Price = shopItem.Price,
                ImageUrl = shopItem.ImageUrl,
                ImageThumbnailUrl = shopItem.ImageThumbnailUrl,
                InStock = shopItem.InStock,
                CategoryId = shopItem.CategoryId,
                Category = shopItem.Category,
                Notes = shopItem.Notes,
                Id = shopItem.Id
            };
            return shopItemModel;
        }
    }
}
