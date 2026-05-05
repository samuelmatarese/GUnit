using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GUnit.Library.Models;

public class TestCase(
    MethodInfo method, 
    object[] parameters = null, 
    Exception encounteredException = null)
{
    public MethodInfo Method {get; set;} = method;
    public object[] Parameters {get; set;} = parameters ?? [];
    public Exception EncounteredException {get; set;} = encounteredException;
}