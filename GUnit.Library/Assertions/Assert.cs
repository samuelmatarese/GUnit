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
}