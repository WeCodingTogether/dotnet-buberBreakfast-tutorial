using BuberBreakfas.Controllers;
using BuberBreakfast.Models;
using BuberBreakfas.Services.Breakfasts;
using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.ServiceErrors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

[ApiController]
// [Route("[Controller]")] // 默认使用controller的名字（BreakfastsController去掉Controller）
public class BreakfastsController : ApiController
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
        ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(request);

        if (requestToBreakfastResult.IsError)
        {
            return Problem(requestToBreakfastResult.Errors);
        }
        var breakfast = requestToBreakfastResult.Value;
        // TODO: sabe breakfast to database
        ErrorOr<Created> createBreakfastresult = _breakfastService.CreateBreakfast(breakfast);
        // return Ok(response);
        // return CreatedAtAction(
        //     actionName: nameof(GetBreakfast),
        //     routeValues: new { id = breakfast.Id },
        //     //value: response
        //     value: MapBreakfastResponse(breakfast)
        // );
        return createBreakfastresult.Match(
            created => CreatedAtGetBreakfast(breakfast),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

        // if (getBreakfastResult.IsError &&
        //     getBreakfastResult.FirstError == Errors.Breakfast.NotFound)
        // {
        //     return NotFound();
        // }

        // Breakfast breakfast = getBreakfastResult.Value;

        // var response =

        // return Ok(response);

        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(id, request);

        if (requestToBreakfastResult.IsError)
        {
            return Problem(requestToBreakfastResult.Errors);
        }
        Breakfast breakfast = requestToBreakfastResult.Value;
        ErrorOr<UpsertedBreakfastResult> upsertBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);

        // TODO: return 201 if a new breakfast was created

        // return NoContent();
        return upsertBreakfastResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        ErrorOr<Deleted> deletedResult = _breakfastService.DeleteBreakfast(id);
        // return NoContent();
        return deletedResult.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }

    /**
        Reduce duplicated code
    */
    private static BreakfastReponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastReponse(
         breakfast.Id,
         breakfast.Name,
         breakfast.Description,
         breakfast.StartDateTime,
         breakfast.EndDateTime,
         breakfast.LastModifiedDateTime,
         breakfast.Savory,
         breakfast.Sweet
        );
    }

    private CreatedAtActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            //value: response
            value: MapBreakfastResponse(breakfast)
        );
    }
}