using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Exceptions
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException() : base("Authentication error occurred!")
        {
        }

        public AuthenticationFailedException(string? message) : base(message)
        {
        }

        public AuthenticationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

    }
}
