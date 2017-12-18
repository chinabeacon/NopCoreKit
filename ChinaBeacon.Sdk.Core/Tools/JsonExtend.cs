namespace ChinaBeacon.Sdk.Core.Tools
{
    /// <summary>
    /// Json 序列化与反序列化的类
    /// </summary>
    public static class JsonExtend
    {
        /// <summary>
        /// 将对象序列化为Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将Json反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
