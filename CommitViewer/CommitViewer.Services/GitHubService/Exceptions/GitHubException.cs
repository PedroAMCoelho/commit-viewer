using System;
using System.Net;
using System.Runtime.Serialization;

namespace CommitViewer.Services.GitHubService.Exceptions
{
    [Serializable]
    public class GitHubException : Exception
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        public GitHubException()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public GitHubException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
        public GitHubException(string message, Exception innerException, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
        public GitHubException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
