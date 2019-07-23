using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COD.Kinesis.Client.Serialization
{
    public abstract class StringBasedSerializer : IMessageSerializer
    {
        public T Deserialize<T>(byte[] array, int length)
        {
            return DeserializeFromString<T>(Encoding.UTF8.GetString(array));
        }

        public T Deserialize<T>(Stream stream, int length)
        {

            return DeserializeFromString<T>(new StreamReader(stream).ReadToEnd());
        }

        public T Deserialize<T>(byte[] array)
        {
            return DeserializeFromString<T>(Encoding.UTF8.GetString(array));
        }

        public T Deserialize<T>(Stream stream)
        {
            return DeserializeFromString<T>(new StreamReader(stream).ReadToEnd());
        }

        public abstract T DeserializeFromString<T>(string value);


        public byte[] SerializeToArray<T>(T value)
        {
            return Encoding.UTF8.GetBytes(SerializeToString(value));
        }

        public void SerializeToStream<T>(Stream stream, T value)
        {
            new StreamWriter(stream).Write(SerializeToString(value));
        }

        public abstract string SerializeToString<T>(T value);
    }
}
