using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Queries;
using Application.Features.Users.Commands;
using Application.Features.Users.Contracts.Requests;
using Application.Features.Users.Queries;
using DataTables.AspNet.Core;
using Domain.Common;
using Domain.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class UsersController(IUnitOfWork unitOfWork,
                            CreateUserCommand createUserCommand,
                            UpdateUserCommand updateUserCommand,
                            DeleteUserCommand deleteUserCommand,
                            GetAllUsersQuery getAllUsersQuery ,
                            GetUserQuery getUserQuery,
                            GetAvailableUsersForTaskQuery getAvailableUsersForTaskQuery ) : ControllerBase
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Edit(Guid? id)
    {
        ViewBag.Id = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
            return HandleResult<bool>(ExtractValidationErrors(ModelState));

        var result = await createUserCommand.Handle(request);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest request, Guid id)
    {
        if (!ModelState.IsValid)
            return HandleResult<bool>(ExtractValidationErrors(ModelState));

        var result = await updateUserCommand.Handle(id,request);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        var result = await deleteUserCommand.Handle(id);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll([FromForm] IDataTablesRequest request)
    {
        var result = await getAllUsersQuery.Handle(request);

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetAvailableUsersForTask([FromBody] GetAvailableUsersForTask request)
    {
        if (!ModelState.IsValid)
            return HandleResult<bool>(ExtractValidationErrors(ModelState));

        var result = await getAvailableUsersForTaskQuery.Handle(request);
        return HandleResult(result);
    }


    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await getUserQuery.Handle(id);

        return HandleResult(result);
    }
}