using Newtonsoft.Json;
using System;

namespace SGcombo.JsonUtils
{


////////////////////////////////////////////////////////////////////////////
//	Copyright 2006 - 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//        
//             https://github.com/Vladimir-Novick/CSharp-Library
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 
    ///   Using:
    ///       Res = JsonConvert.DeserializeObject<MyCalendar>(sResponse, new CustomJsonConverterDouble());
    /// 
    /// </summary>
    public class CustomJsonConverterDouble : JsonConverter
    {
        /// <summary>
        ///    
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(double); // convert only double data type
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(double))
            {
                Double d = 0;

                if (reader.Value != null)
                {
                    String str = reader.Value.ToString();
                    Double.TryParse(str, out d);
                }
                return d;
            }
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
