using System;

namespace IntegrationTest.EntryPoint.WebApi.Commons.Assertions;

public class AssertionException : Exception
{
    public override string StackTrace { get; }

    public AssertionException(string stackTrace, string message = null) : base(message)
    {
        StackTrace = stackTrace;
    }
}