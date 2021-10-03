using System;
using System.Net;
using System.Runtime.Serialization;

namespace CommitViewer.Services.GitCliService.Exceptions
{
    [Serializable]
    public class GitCliServiceException : Exception
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        public GitCliServiceException()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public GitCliServiceException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
        public GitCliServiceException(string message, Exception innerException, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
        public GitCliServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
