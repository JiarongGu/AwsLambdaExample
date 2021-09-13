using System;

namespace AwsLambdaExample.Application.Exceptions
{
    public class KnownException : Exception
    {
        public KnownException(string message) : base(message) { }
    }
}
