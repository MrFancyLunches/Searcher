using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearcher
{
    public static class Serializer
    {

        /// <summary>
        ///     Serialize value to json formatted string
        /// </summary>
        /// <param name="o">Object to serialize</param>
        /// <param name="value">Resulting string</param>
        /// <returns>Success</returns>
        public static bool ToJson<T>(T o, out string value)
        {
            try
            {
                value = JsonConvert.SerializeObject(o);
            }
            catch (Exception e)
            {
                value = string.Empty;
                Logger.LogError("Failed to serialize " + o.GetType() + " to json: " + e.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        ///     Deserializes json formatted string to object
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="json">Json formatted string</param>
        /// <param name="result">Resulting object</param>
        /// <returns>Success</returns>
        public static bool FromJson<T>(string json, out T result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch(Exception e)
            {
                result = default;
                Logger.LogError("Failed to deserialize json to " + result.GetType() + ": " + e.Message);
                return false;
            }
            return true;
        }
    }
}
