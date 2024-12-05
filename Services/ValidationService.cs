using System.ComponentModel.DataAnnotations;

namespace Arvefordeleren.Services;

public static class ValidationService
{

    public static bool IsAllFieldsValid<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
            return false;
        
        foreach (var item in list)
        {
            var context = new ValidationContext(item, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(item, context, results, true))
            {
                return false;
            }
        }

        return list.Count != 0;
    }
    
}