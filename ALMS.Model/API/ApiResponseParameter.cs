using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ALMS.Model
{
    public class ApiResponseParameter<TResult>
    {

        /// <summary>
        /// Http Status Code
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(StringEnumConverter))]
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Result Type
        /// </summary>
        /// <value>Success</value>
        /// <value>Fail</value>
        /// <value>NoContent</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public ResultType ResultType { get; set; }

        /// <summary>
        /// Kullanıcı Mesajı
        /// </summary>
        /// <value></value>
        public string Message { get; set; }

        /// <summary>
        /// Return Value
        /// </summary>
        /// <value></value>
        public TResult DataModel { get; set; }

        /// <summary>
        /// Internal Messages
        /// </summary>
        /// <value></value>
        public List<ErrorMessage> InternalMessages { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.None
            });
        }
    }
}