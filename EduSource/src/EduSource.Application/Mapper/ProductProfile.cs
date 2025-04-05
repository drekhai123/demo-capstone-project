using AutoMapper;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Domain.Entities;
using static EduSource.Contract.Services.Books.Response;
using static EduSource.Contract.Services.Products.Response;

namespace EduSource.Application.Mapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponse>();
        CreateMap<PagedResult<Product>, PagedResult<ProductResponse>>();
    }
}
