using AutoMapper;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Domain.Entities;
using static EduSource.Contract.Services.Books.Response;

namespace EduSource.Application.Mapper;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookResponse>();
        CreateMap<PagedResult<Book>, PagedResult<BookResponse>>();
    }
}
