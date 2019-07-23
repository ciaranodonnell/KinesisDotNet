using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace COD.Kinesis.Client
{
    /// <summary>
    /// This is an idea about creating a producer that doesnt need re-initializing
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    internal interface IMessageProducer<TMessage>
    {

        Task SendMessageAsync(TMessage message);

        void SendMessage(TMessage message);


    }
}
