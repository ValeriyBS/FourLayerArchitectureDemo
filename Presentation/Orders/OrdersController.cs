﻿using System.Linq;
using System.Threading.Tasks;
using Application.Orders.Commands.CreateOrder;
using Application.ShoppingCartItems.Queries.GetShoppingCartItemsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Orders.Models;
using Presentation.Orders.Services.Commands.SaveApplicationUser;
using Presentation.Orders.Services.Queries;
using Presentation.Orders.Services.Queries.GetApplicationUser;
using Presentation.ShoppingCarts.Services.Queries;

namespace Presentation.Orders
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly CartIdProvider _cartIdProvider;
        private readonly IGetShoppingCartItemsListQuery _getShoppingCartItemsListQuery;
        private readonly ICreateOrderCommand _createOrderCommand;
        private readonly IGetApplicationUserDetails _getApplicationUserDetails;
        private readonly IGetApplicationUserId _getApplicationUserId;
        private readonly ISaveApplicationUserDetails _saveApplicationUserDetails;

        public OrdersController(CartIdProvider cartIdProvider,
            IGetShoppingCartItemsListQuery getShoppingCartItemsListQuery,
            ICreateOrderCommand createOrderCommand,
            IGetApplicationUserDetails getApplicationUserDetails,
            IGetApplicationUserId getApplicationUserId,
            ISaveApplicationUserDetails saveApplicationUserDetails)
        {
            _cartIdProvider = cartIdProvider;
            _getShoppingCartItemsListQuery = getShoppingCartItemsListQuery;
            _createOrderCommand = createOrderCommand;
            _getApplicationUserDetails = getApplicationUserDetails;
            _getApplicationUserId = getApplicationUserId;
            _saveApplicationUserDetails = saveApplicationUserDetails;
        }

        public async Task<IActionResult> Checkout()
        {
            var viewModel = await _getApplicationUserDetails.Execute(HttpContext.User);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Checkout(CreateOrderViewModel viewModel)
        {
            if (!ModelState.IsValid) return View();

            var shoppingCartItems = _getShoppingCartItemsListQuery.Execute(_cartIdProvider.CartId);

            if (!shoppingCartItems.Any()) ModelState.AddModelError("", "You cart is empty.Add some goods first");

            var userId = _getApplicationUserId.Execute(HttpContext.User);

            _saveApplicationUserDetails.Execute(userId,viewModel);

            var orderModel = new CreateOrderModel(userId, _cartIdProvider.CartId);

            _createOrderCommand.Execute(orderModel);

            return RedirectToAction("CheckoutComplete");
        }


        public IActionResult CheckoutComplete()
        {
            return View();
        }
    }
}