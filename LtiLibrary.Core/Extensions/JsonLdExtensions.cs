﻿using System;
using System.IO;
using System.Net;
using LtiLibrary.Core.Common;
using Newtonsoft.Json;
using System.Net.Http;

namespace LtiLibrary.Core.Extensions
{
    public static class JsonLdExtensions
    {
        /// <summary>
        /// Serializes the specified object to a JSON string using default contract resolver.
        /// </summary>
        /// <remarks>
        /// This should only be used for inbound requests for pretty display. Use ToJsonLdString
        /// to format outbound requests.
        /// </remarks>
        public static string ToJsonString<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using a JSON-LD contract resolver.
        /// </summary>
        /// <remarks>
        /// This should only be used for outbound requests. Use ToJsonString to format inbound
        /// requests for pretty display.
        /// </remarks>
        public static string ToJsonLdString<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new JsonLdObjectContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        /// <summary>
        /// Deserializes the JSON response to the specified .NET type.
        /// </summary>
        public static T DeserializeObject<T>(this HttpResponseMessage response)
        {
            try
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return default(T);
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        var body = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<T>(body);
                    }
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
