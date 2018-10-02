using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string errorMessage = "The Server has encountered an error.";

        public const HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;

        public InternalServerErrorException()
            :base(errorMessage)
        {
        }
    }
}
