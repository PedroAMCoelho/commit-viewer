using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CommitViewer.Shared.Extensions
{
    public static class JsonExtensions
    {
        public async static Task<T> StringJsonToMap<T>(this string strJson) where T : class
        {
            return JsonConvert.DeserializeObject<T>(strJson);
        }
    }
}
