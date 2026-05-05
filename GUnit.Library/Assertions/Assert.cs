using System;

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
    {
        if (!expected.Equals(actualResult))
            throw new Exception("Assert.Equal failed: " + message);
    }
    
    public static void OfType<T>(object actualResult, string message = "")
    {
        if (!(actualResult.GetType() == typeof(T)))
            throw new Exception("Assert.OfType failed: " + message);
    }
}