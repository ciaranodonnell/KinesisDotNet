namespace DemoApp.EventProducer
{
    internal class DemoAppEvent
    {
        public int CustomerId { get; internal set; }
        public string CustomerName { get; internal set; }

        public string OtherInfo { get; set; }

    }
}