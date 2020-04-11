using System;

namespace PlanerUek.Website.Exceptions
{
    public class GoogleAuthorizationException : Exception
    {
        public GoogleAuthorizationException(string message) : base(message)
        {
        }
    }
}