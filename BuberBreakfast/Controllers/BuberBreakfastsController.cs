using BuberBreakfas.Models;
using BuberBreakfas.Services.Breakfasts;
using BuberBreakfast.Contracts.Breakfast;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

[ApiController]
[Route("[Controller]")] // 默认使用controller的名字（BreakfastsController去掉Controller）
public class BreakfastsController : ControllerBase
{
    private readonly IBreakfastService _breakfastService;

    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    // [HttpPost("/breakfasts")]
    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        // 接收post的request对象，转为breakfast对象
        var breakfast = new Breakfast(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow,
            request.Savory,
            request.Sweet
        );

        // TODO: sabe breakfast to database
        _breakfastService.CreateBreakfast(breakfast);

        // 返回breakfastResponse对象
        var response = new BreakfastReponse(
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
        );

        // return Ok(response);
        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: response
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        Breakfast breakfast = _breakfastService.GetBreakfast(id);
        var response = new BreakfastReponse(
         breakfast.Id,
         breakfast.Name,
         breakfast.Description,
         breakfast.StartDateTime,
         breakfast.EndDateTime,
         breakfast.LastModifiedDateTime,
         breakfast.Savory,
         breakfast.Sweet
        );
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        return Ok(request);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        return Ok(id);
    }
}