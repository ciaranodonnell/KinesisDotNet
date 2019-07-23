using Amazon.Kinesis.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Amazon.Kinesis.ClientLibrary
{

    /// <summary>
    /// Provides a simple CheckpointErrorHandler that retries the checkpoint operation a number of times,
    /// with a fixed delay in between each attempt.
    /// </summary>
    public static class RetryingCheckpointErrorHandler
    {
        /// <summary>
        /// Create a simple CheckpointErrorHandler that retries the checkpoint operation a number of times,
        /// with a fixed delay in between each attempt.
        /// </summary>
        /// <param name="retries">Number of retries to perform before giving up.</param>
        /// <param name="delay">Delay between each retry.</param>
        public static CheckpointErrorHandler Create(int retries, TimeSpan delay)
        {
            return (seq, err, checkpointer) =>
            {
                if (retries > 0)
                {
                    Thread.Sleep(delay);
                    checkpointer.Checkpoint(seq, Create(retries - 1, delay));
                }
            };
        }
    }

}
