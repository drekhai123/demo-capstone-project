using AutoMapper;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Books;
using EduSource.Domain.Abstraction.Dappers;
using static EduSource.Contract.Services.Books.Response;

namespace EduSource.Application.UseCases.V1.Queries.Books;

public sealed class GetAllBooksQueryHandler : IQueryHandler<Query.GetAllBooksQuery, Success<PagedResult<BookResponse>>>
{
    private readonly IDPUnitOfWork _dpUnitOfWork;
    private readonly IMapper _mapper;

    public GetAllBooksQueryHandler(IDPUnitOfWork dpUnitOfWork, IMapper mapper)
    {
        _dpUnitOfWork = dpUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<Success<PagedResult<BookResponse>>>> Handle(Query.GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        //Find All Books
        var listBooks = await _dpUnitOfWork.BookRepositories.GetPagedAsync(request.PageIndex, request.PageSize, request.FilterParams, request.SelectedColumns);
        //Mapping Category to CategoryResponse
        var result = _mapper.Map<PagedResult<BookResponse>>(listBooks);
        //Check if ListCategory is empty
        if (listBooks.Items.Count == 0)
        {
            return Result.Success(new Success<PagedResult<BookResponse>>(MessagesList.BookNotFoundException.GetMessage().Code, MessagesList.BookNotFoundException.GetMessage().Message, result));
        }
        //Return result
        return Result.Success(new Success<PagedResult<BookResponse>>(MessagesList.BookGetAllBooksSuccess.GetMessage().Code, MessagesList.BookGetAllBooksSuccess.GetMessage().Message, result));
    }
}
