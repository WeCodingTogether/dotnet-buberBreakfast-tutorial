using System.ComponentModel;
using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Models;

public class Breakfast
{
    //Validation conditions
    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;

    public const int MinDescriptionLength = 50;
    public const int MaxDescriptionLength = 150;

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public DateTime LastModifiedDateTime { get; }
    public List<string> Savory { get; }
    public List<string> Sweet { get; }

    // constructor
    private Breakfast(
        Guid id,
        string name,
        string des,
        DateTime StartDateTime,
        DateTime EndDateTime,
        DateTime LastModifiedDateTime,
        List<string> Savory,
        List<string> Sweet
    )
    {
        Id = id;
        Name = name;
        Description = des;
        this.StartDateTime = StartDateTime;
        this.EndDateTime = EndDateTime;
        this.LastModifiedDateTime = LastModifiedDateTime;
        this.Savory = Savory;
        this.Sweet = Sweet;
    }

    public static ErrorOr<Breakfast> Create(
        string name,
        string des,
        DateTime startDateTime,
        DateTime endDateTime,
        List<string> savory,
        List<string> sweet,
        Guid? id = null
    )
    {
        List<Error> errors = new();
        // name length validation
        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Breakfast.InvalidName);
        }
        // description length vilidation
        if (des.Length is < MinDescriptionLength or > MaxDescriptionLength)
        {
            errors.Add(Errors.Breakfast.InvalidDescription);
        }
        if (errors.Count > 0)
        {
            return errors;
        }

        return new Breakfast(
            id ?? Guid.NewGuid(),
            name,
            des,
            startDateTime,
            endDateTime,
            DateTime.UtcNow,
            savory,
            sweet
        );
    }

    internal static ErrorOr<Breakfast> From(CreateBreakfastRequest request)
    {
        return Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet
        );
    }

    internal static ErrorOr<Breakfast> From(Guid id, UpsertBreakfastRequest request)
    {
        return Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet,
            id
        );
    }
}