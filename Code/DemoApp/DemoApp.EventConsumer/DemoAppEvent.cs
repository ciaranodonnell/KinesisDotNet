namespace DemoApp.EventConsumer
{
    public class DemoAppEvent
    {
        public int CustomerId { get; internal set; }
        public string CustomerName { get; internal set; }

        public string OtherInfo { get; set; }

    }
}