using Asp.Versioning;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Services.Books;
using EduSource.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduSource.Contract.Services.Books.Filter;

namespace EduSource.Presentation.Controller.V1
{
    [ApiVersion(1)]
    public class BookController : ApiController
    {
        public BookController(ISender sender) : base(sender)
        {
        }
        [HttpGet("get_all_books", Name = "GetAllBooks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<Success>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<Error>))]
        public async Task<IActionResult> GetAllBooks([FromQuery] BookFilter filterParams,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string[] selectedColumns = null)
        {
            var result = await Sender.Send(new Query.GetAllBooksQuery(pageIndex, pageSize, filterParams, selectedColumns));
            if (result.IsFailure)
                return HandlerFailure(result);

            return Ok(result);
        }
    }
}
