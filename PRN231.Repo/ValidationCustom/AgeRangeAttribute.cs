using System.ComponentModel.DataAnnotations;

namespace PRN231.Repo.ValidationCustom;

public class AgeRangeAttribute : ValidationAttribute
{
    private readonly int _minAge;
    private readonly int _maxAge;

    public AgeRangeAttribute(int minAge, int maxAge)
    {
        _minAge = minAge;
        _maxAge = maxAge;
    }

    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            int age = DateTime.Today.Year - date.Year;
            if (date > DateTime.Today.AddYears(-age)) age--;
            return age >= _minAge && age <= _maxAge;
        }
        return false;
    }
}