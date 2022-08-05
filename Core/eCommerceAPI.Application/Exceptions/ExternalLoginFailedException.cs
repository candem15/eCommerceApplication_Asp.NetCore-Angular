using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Exceptions
{
    public class ExternalLoginFailedException : Exception
    {
        public ExternalLoginFailedException() : base("Invalid external authentication!")
        {
        }

        public ExternalLoginFailedException(string? message) : base(message)
        {
        }

        protected ExternalLoginFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
