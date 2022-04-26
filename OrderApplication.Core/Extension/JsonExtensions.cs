using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Core.Extension
{
    public static class JsonExtensions
    {
        public static string ToJSON<T>(this T value, bool apostropheCheck = false, bool isNullableIgnore = false)
        {
            if (value == null) return string.Empty;

            string json = string.Empty;

            if (isNullableIgnore)
            {
                json = JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                {
                    MaxDepth = Int32.MaxValue,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                {
                    MaxDepth = Int32.MaxValue,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                });
            }

            json = apostropheCheck ? json.Replace("'", "\\'").Replace("\"", "\\\"") : json;
            json = json.RemoveRepeatedWhiteSpace();
            json = json.Replace(Environment.NewLine, string.Empty);
            return json;
        }

        private static string RemoveRepeatedWhiteSpace(this string value)
        {
            if (value.IsNull()) return string.Empty;

            string[] tmp = value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (string word in tmp)
            {
                sb.Append(word.Replace("\r", "").Replace("\n", "").Replace("\t", "")/*.Replace("\\" , "")*/ + " ");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
