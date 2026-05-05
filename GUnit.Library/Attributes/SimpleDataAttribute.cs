using System;

namespace GUnit.Library.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class SimpleDataAttribute(params object[] data) : Attribute
{
    public object[] Data { get; } = data;
}