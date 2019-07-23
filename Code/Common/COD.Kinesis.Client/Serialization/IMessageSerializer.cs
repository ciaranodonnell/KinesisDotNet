using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COD.Kinesis.Client.Serialization
{
    public interface IMessageSerializer
    {

        T Deserialize<T>(byte[] array, int length);
        T Deserialize<T>(Stream stream, int length);


        T Deserialize<T>(byte[] array);
        T Deserialize<T>(Stream stream);


        byte[] SerializeToArray<T>(T value);

        string SerializeToString<T>(T value);
        void SerializeToStream<T>(Stream stream, T value);


    }
}
