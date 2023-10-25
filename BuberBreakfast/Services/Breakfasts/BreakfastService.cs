using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfas.Services.Breakfasts;

public class BreakfastService : IBreakfastService
{
    private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();

    public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    {
        _breakfasts.Add(breakfast.Id, breakfast);
        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        if (_breakfasts.ContainsKey(id))
        {
            _breakfasts.Remove(id);
            return Result.Deleted;
        }

        return Errors.Breakfast.NotFound;
    }

    public ErrorOr<Breakfast> GetBreakfast(Guid id)
    {
        if (_breakfasts.TryGetValue(id, out var breakfast))
        {
            return breakfast;
        }
        // return _breakfasts[id]; // 通过key，拿到value
        return Errors.Breakfast.NotFound;
    }

    public ErrorOr<UpsertedBreakfastResult> UpsertBreakfast(Breakfast breakfast)
    {
        // _breakfasts[breakfast.Id] = breakfast;
        // return Result.Updated;

        var IsNewlyCreated = !_breakfasts.ContainsKey(breakfast.Id);
        _breakfasts[breakfast.Id] = breakfast;

        return new UpsertedBreakfastResult(IsNewlyCreated);
    }
}
