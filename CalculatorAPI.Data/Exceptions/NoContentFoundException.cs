using System;

namespace CalculatorAPI.Data.Exceptions
{
    public class NoContentFoundException : Exception
    {
        public NoContentFoundException() : base("No content found.")
        {
        }

        public NoContentFoundException(string message) : base(message)
        {
        }

        public NoContentFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}