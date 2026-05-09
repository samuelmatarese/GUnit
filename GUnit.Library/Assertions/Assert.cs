using System;
using System.Text;

namespace GUnit.Library.Assertions;

public static class Assert
{
    public static void True(bool condition, string message = "")
    {
        if (!condition)
            throw new Exception("Assert.True failed: " + message);
    }

    public static void NotNull(object obj, string message = "")
    {
        if (obj == null)
            throw new Exception("Assert.NotNull failed: " + message);
    }  
    
    public static void Equal<T>(T expected, T actualResult, string message = "")
        where T : notnull
    {
        if (!expected.Equals(actualResult))
            throw new Exception("Assert.Equal failed: " + message);
    }

    public static void OfType<T>(object actualResult)
    {
        OfType(typeof(T), actualResult);
    }
    
    public static void OfType(Type expectedType, object actualResult)
    {
        if (actualResult.GetType() != expectedType)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Assert.OfType failed:");
            stringBuilder.AppendLine($"ExpectedType: '{expectedType.Name}'");
            stringBuilder.AppendLine($"ActualType: '{actualResult.GetType().Name}'");

            throw new Exception(stringBuilder.ToString());
        }
    }    
}