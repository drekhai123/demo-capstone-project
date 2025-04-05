using Asp.Versioning;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.DTOs.ProductDTOs;
using EduSource.Contract.Services.Products;
using EduSource.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static EduSource.Contract.DTOs.ProductDTOs.ProductRequestDTO;
using static EduSource.Contract.Services.Products.Filter;

namespace EduSource.Presentation.Controller.V1;

[ApiVersion(1)]
public class ProductController : ApiController
{
    public ProductController(ISender sender) : base(sender)
    {
    }
    [HttpGet("get_all_products", Name = "GetAllProducts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductRequestDTO request,
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string[] selectedColumns = null)
    {
        var filterParams = new ProductFilter(request.Name, request.Price, request.Category, request.Description, request.ContentType, request.Unit, request.UploadType, request.TotalPage, request.Size, request.Rating, request.IsPublic, request.IsApproved, null, request.BookId, null);
        
        var result = await Sender.Send(new Query.GetAllProductsQuery(pageIndex, pageSize, filterParams, selectedColumns));
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [Authorize (Policy = "MemberPolicy")]
    [HttpGet("get_all_products_by_user", Name = "GetAllProductsByUser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> GetAllProductsByUser([FromQuery] GetAllProductRequestDTO request,
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string[] selectedColumns = null)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var filterParams = new ProductFilter(request.Name, request.Price, request.Category, request.Description, request.ContentType, request.Unit, request.UploadType, request.TotalPage, request.Size, request.Rating, request.IsPublic, request.IsApproved, null, request.BookId, userId);

        var result = await Sender.Send(new Query.GetAllProductsByUserQuery(pageIndex, pageSize, filterParams, selectedColumns));
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpGet("get_product_by_id", Name = "GetProductById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> GetProductById([FromQuery] Query.GetProductByIdQuery Queries)
    {
        var result = await Sender.Send(Queries);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [Authorize(Policy = "MemberPolicy")]
    [HttpGet("get_product_by_id_by_user", Name = "GetProductByIdByUser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> GetProductByIdByUser([FromQuery] Guid Id)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var result = await Sender.Send(new Query.GetProductByIdByUserQuery(Id, userId));
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [Authorize(Policy = "StaffPolicy")]
    [HttpPost("create_product", Name = "CreateProduct")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequestDTO productRequestDTO)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var result = await Sender.Send(new Command.CreateProductCommand(productRequestDTO.Name, productRequestDTO.Price, productRequestDTO.Category, productRequestDTO.Description, productRequestDTO.ContentType, productRequestDTO.Unit, productRequestDTO.UploadType, productRequestDTO.TotalPage, productRequestDTO.Size, productRequestDTO.MainImage, productRequestDTO.File, productRequestDTO.FileDemo, productRequestDTO.OtherImages, productRequestDTO.BookId, userId));
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [Authorize(Policy = "MemberPolicy")]
    [HttpGet("get_all_products_purchased", Name = "GetAllProductsPurchased")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
    public async Task<IActionResult> GetAllProductsPurchased([FromQuery] GetAllProductPurchasedRequestDTO request,
    [FromQuery] int pageIndex = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string[] selectedColumns = null)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var filterParams = new ProductFilter(request.Name, request.Price, request.Category, request.Description, request.ContentType, request.Unit, request.UploadType, request.TotalPage, request.Size, request.Rating, request.IsPublic, request.IsApproved, null, request.BookId, userId);

        var result = await Sender.Send(new Query.GetAllProductsPurchasedQuery(pageIndex, pageSize, filterParams, selectedColumns));
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }
}
