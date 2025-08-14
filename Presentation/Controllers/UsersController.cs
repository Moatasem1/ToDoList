using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Users.Commands;
using Application.Features.Users.Contracts.Requests;
using Application.Features.Users.Queries;
using Domain.Common;
using Domain.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Services;

namespace Presentation.Controllers;

// [ApiController]
// [Route("api/[controller]")]
public class UsersController(IUnitOfWork unitOfWork,CreateUserCommand createUserCommand, UpdateUserCommand updateUserCommand, DeleteUserCommand deleteUserCommand, GetAllUsersQuery getAllUsersQuery , GetUserQuery getUserQuery ) : ControllerBase
{

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var result = await createUserCommand.Handle(request);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(UpdateUserRequest request, Guid id)
    {
        var result = await updateUserCommand.Handle(id,request);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await deleteUserCommand.Handle(id);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await getAllUsersQuery.Handle();

        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await getUserQuery.Handle(id);

        return HandleResult(result);
    }
}