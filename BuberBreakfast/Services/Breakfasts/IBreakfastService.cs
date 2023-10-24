using BuberBreakfas.Models;

namespace BuberBreakfas.Services.Breakfasts;

public interface IBreakfastService
{
    void CreateBreakfast(Breakfast breakfast);
    Breakfast GetBreakfast(Guid id);
}