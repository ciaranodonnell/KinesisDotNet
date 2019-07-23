using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace COD.Kinesis.Client.Serialization
{
    public class JSONSerializer : StringBasedSerializer
    {
        public override T DeserializeFromString<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public override string SerializeToString<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
