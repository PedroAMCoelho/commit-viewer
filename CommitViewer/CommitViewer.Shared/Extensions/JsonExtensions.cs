using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommitViewer.Shared.Extensions
{
    /// <summary>
    /// The json extensions.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Responses the message to map.
        /// </summary>
        /// <param name="httpResponseMessage">The http response message.</param>
        /// <returns>A Task.</returns>
        public async static Task<T> ResponseMessageToMap<T>(this HttpResponseMessage httpResponseMessage) where T : class
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
