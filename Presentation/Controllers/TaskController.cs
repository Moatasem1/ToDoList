using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Contracts.Requests;
using Domain.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(CreateTaskCommand createTask, UpdateTaskCommand updateTaskCommand ,DeleteTaskCommand deleteTaskCommand , IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var result = await createTask.Handle(request);

        if (result.IsSuccess) 
            await unitOfWork.SaveChangesAsync();

        return HandleResult<Guid>(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateTaskRequest request)
    {
        var result = await updateTaskCommand.Handle(request);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult<bool>(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteTaskRequest request)
    {
        var result = await deleteTaskCommand.Handle(request);

        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync();

        return HandleResult<bool>(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        //var result = await deleteTaskCommand.Handle(request);

        //if (result.IsSuccess)
        //    await unitOfWork.SaveChangesAsync();

        //return HandleResult<bool>(result);
    }
}