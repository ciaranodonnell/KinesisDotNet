using COD.Kinesis.Client;
using COD.Kinesis.Client.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.EventProducer
{
    public class Program
    {

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public async static Task MainAsync(string[] args)
        {
            KinesisClient client = new KinesisClient("DemoApp", "us-east-1");

            var serializer = new JSONSerializer();
            var sender = client.GetMessageSender<DemoAppEvent>("eventstream1", serializer, (m) => m.CustomerId.ToString());

            Random r = new Random();
            for (int x = 0; x < 10; x++)
            {
                var message = new DemoAppEvent { CustomerId = r.Next(short.MaxValue), CustomerName = "Customer " + x.ToString(), OtherInfo = "Random other info" };
                Console.WriteLine(serializer.SerializeToString(message));
                await sender.SendMessageAsync(message);

            }

            Console.WriteLine();
            Console.WriteLine("all messages send");
            Console.ReadLine();


        }
    }
}