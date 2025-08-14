using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Queries;
using Domain.Common;
using Domain.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Services;

namespace Presentation.Controllers;

//[ApiController]
//[Route("api/[controller]")]
public class TasksController(CreateTaskCommand createTask, UpdateTaskCommand updateTaskCommand, DeleteTaskCommand deleteTaskCommand, CurrentUserService currentUserService, GetTasks getTasks, UpdateTaskStatusCommand updateTaskStatusCommand, IUnitOfWork unitOfWork) : ControllerBase
{
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// create task and assign users to it
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        if (currentUserService.UserId == null)
            return HandleResult<bool>(Error.Unauthorized());

        var result = await createTask.Handle(request, currentUserService.UserId.Value);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    /// <summary>
    /// update task and adds new user assignments and removes any users who are no longer assigned to the task
    /// status will not update
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskRequest request)
    {
        if (currentUserService.UserId == null)
            return HandleResult<bool>(Error.Unauthorized());

        var result = await updateTaskCommand.Handle(request, currentUserService.UserId.Value, id);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (currentUserService.UserId == null)
            return HandleResult<bool>(Error.Unauthorized());

        var result = await deleteTaskCommand.Handle(currentUserService.UserId.Value, id);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetTaskFilter taskFilter)
    {
        if (currentUserService.UserId == null)
            return HandleResult<bool>(Error.Unauthorized());

        var result = await getTasks.Handle(currentUserService.UserId.Value, taskFilter);

        return HandleResult(result);
    }


    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTaskStatus(UpdateTaskStatusRequest request, Guid id)
    {
        if (currentUserService.UserId == null)
            return HandleResult<bool>(Error.Unauthorized());

        var result = await updateTaskStatusCommand.Handle(request, currentUserService.UserId.Value, id);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult(result);
    }
}