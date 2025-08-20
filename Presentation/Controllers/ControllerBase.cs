using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Presentation.Options;

namespace Presentation.Controllers;

public abstract class ControllerBase : Controller
{
    protected IActionResult HandleResult<T>(Result<T, Error> result)
    {
        if (result.IsSuccess)
            return Ok(ResponseEnvelope<T>.Success(result.Value));

        return GetErorr(result);
    }

    public static Error ExtractValidationErrors(ModelStateDictionary modelState)
    {
        return modelState
            .Where(e => e.Value?.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors
                .Select(err => Error.ValueInvalidWithMessage(e.Key, err.ErrorMessage)))
            .First();
    }


    private IActionResult GetErorr<T>(Result<T, Error> result)
    {
        return result.Error.ErrorType switch
        {
            ErrorType.NotFound => NotFound(ResponseEnvelope<T>.Fail([result.Error])),
            ErrorType.Conflict => Conflict(ResponseEnvelope<T>.Fail([result.Error])),
            ErrorType.Validation => BadRequest(ResponseEnvelope<T>.Fail([result.Error])),
            ErrorType.Unauthorized => Unauthorized(ResponseEnvelope<T>.Fail([result.Error])),
            _ => BadRequest(ResponseEnvelope<T>.Fail([result.Error]))
        };
    }
}