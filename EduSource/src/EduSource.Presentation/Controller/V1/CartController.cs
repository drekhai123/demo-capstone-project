using Asp.Versioning;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Services.Carts;
using EduSource.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static EduSource.Contract.DTOs.CartDTOs.CartRequestDTO;
using static EduSource.Contract.Services.Products.Filter;

namespace EduSource.Presentation.Controller.V1;

[ApiVersion(1)]
public class CartController : ApiController
{
    public CartController(ISender sender) : base(sender)
    {
    }

    [Authorize(Policy = "MemberPolicy")]
    [HttpPost("add_product_to_cart", Name = "AddProductToCart")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> AddProductToCart([FromQuery] Guid productId)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var result = await Sender.Send(new Command.AddProductToCartCommand(productId, userId));
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [Authorize(Policy = "MemberPolicy")]
    [HttpGet("get_all_products_from_cart", Name = "GetAllProductsFromCart")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> GetAllBooks([FromQuery] GetAllProductRequestDTO request,
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string[] selectedColumns = null)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var filterParams = new ProductFilter(request.Name, request.Price, request.Category, request.Description, request.ContentType, request.Unit, request.UploadType, request.TotalPage, request.Size, request.Rating, true, true, null, request.BookId, userId);

        var result = await Sender.Send(new Query.GetAllProductsFromCartQuery(pageIndex, pageSize, filterParams, selectedColumns));
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [Authorize(Policy = "MemberPolicy")]
    [HttpDelete("remove_product_from_cart", Name = "RemoveProductFromCart")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> RemoveProductFromCart(Guid productId)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var result = await Sender.Send(new Command.RemoveProductFromCart(productId, userId));
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
