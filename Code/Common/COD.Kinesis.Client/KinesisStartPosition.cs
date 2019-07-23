
namespace COD.Kinesis.Client
{
    public enum KinesisStartPosition
    {
        /// <summary>
        /// The oldest record in the stream
        /// </summary>
        TRIM_HORIZON,
        /// <summary>
        /// The newest record in the stream - the one written most recently
        /// </summary>
        LATEST
    }
}