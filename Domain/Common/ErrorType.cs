namespace Domain.Common;

public enum ErrorType
{
    InternalError = 1,
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    BadRequest
}