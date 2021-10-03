using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public async static Task<T> StringJsonToMap<T>(this string strJson) where T : class
        {
            return JsonConvert.DeserializeObject<T>(strJson);
        }

        public static T ResolveJson<T>(this JObject jobj, string target)
        {
            return JsonConvert.DeserializeObject<T>(jobj.SelectToken(target).ToString());
        }
    }
}
