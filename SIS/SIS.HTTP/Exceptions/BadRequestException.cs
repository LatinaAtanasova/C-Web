using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SIS.HTTP.Exceptions
{
    public class BadRequestException : Exception
    {

        public const HttpStatusCode StatusCode = HttpStatusCode.BadRequest; 

        private const string errorMessage = "The Request was malformed or contains unsupported elements.";

        public BadRequestException()
            :base(errorMessage)
        {
        }
    }
}
