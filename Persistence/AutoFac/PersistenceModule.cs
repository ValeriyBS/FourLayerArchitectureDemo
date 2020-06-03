﻿using Application.Interfaces.Persistence;
using Autofac;
using Autofac.Features.AttributeFilters;
using Common.Dates;
using Domain.Categories;
using Domain.ShopItems;
using Domain.ShoppingCartItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Categories;
using Persistence.Shared;
using Persistence.ShopItems;
using Persistence.ShoppingCartItems;

namespace Persistence.AutoFac
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseContext>().As<IDatabaseContext>()
                .WithParameter("options", GetDbContextOptions.Execute())
                .InstancePerDependency();

            builder.RegisterType<Repository<Category>>().As<IRepository<Category>>().InstancePerDependency();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerDependency();

            builder.RegisterType<Repository<ShopItem>>().As<IRepository<ShopItem>>().InstancePerDependency();
            builder.RegisterType<ShopItemRepository>().As<IShopItemRepository>().InstancePerDependency();

            builder.RegisterType<Repository<ShoppingCartItem>>().As<IRepository<ShoppingCartItem>>()
                .InstancePerDependency();
            builder.RegisterType<ShoppingCartItemRepository>().As<IShoppingCartItemRepository>()
                .InstancePerDependency();
        }

    }
}