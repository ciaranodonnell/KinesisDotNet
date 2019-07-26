using Amazon.Kinesis.ClientLibrary;
using COD.Kinesis.Client.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace COD.Kinesis.Client
{
    public abstract class SimpleRecordProcessor<TMessage> : IShardRecordProcessor where TMessage : class
    {
        /// <value>The time to wait before this record processor
        /// reattempts either a checkpoint, or the processing of a record.</value>
        protected TimeSpan Backoff { get; set; } = TimeSpan.FromSeconds(3);

        /// <value>The interval this record processor waits between
        /// doing two successive checkpoints.</value>
        protected TimeSpan CheckpointInterval { get; set; } = TimeSpan.FromMinutes(1);
        
        /// <value>The shard ID on which this record processor is working.</value>
        protected string KinesisShardId { get; set; }

        protected int NumOfRetries { get; set; } = 5;

        /// <value>The next checkpoint time expressed in milliseconds.</value>
        private DateTime _nextCheckpointTime = DateTime.UtcNow;
        private IMessageSerializer serializer;

        protected SimpleRecordProcessor(IMessageSerializer serializer)
        {
            this.serializer = serializer;
        }

        /// <summary>
        /// This method is invoked by the Amazon Kinesis Client Library before records from the specified shard
        /// are delivered to this SampleRecordProcessor.
        /// </summary>
        /// <param name="input">
        /// InitializationInput containing information such as the name of the shard whose records this
        /// SampleRecordProcessor will process.
        /// </param>
        public void Initialize(InitializationInput input)
        {
            Console.Error.WriteLine("Initializing record processor for shard: " + input.ShardId);
            this.KinesisShardId = input.ShardId;
        }

        /// <summary>
        /// This method processes the given records and checkpoints using the given checkpointer.
        /// </summary>
        /// <param name="input">
        /// ProcessRecordsInput that contains records, a Checkpointer and contextual information.
        /// </param>
        public void ProcessRecords(ProcessRecordsInput input)
        {
            // Process records and perform all exception handling.
            ProcessReceivedRecords(input.Records);

            // Checkpoint once every checkpoint interval.
            if (DateTime.UtcNow >= _nextCheckpointTime)
            {
                Checkpoint(input.Checkpointer);
                _nextCheckpointTime = DateTime.UtcNow + CheckpointInterval;
            }
        }

        /// <summary>
        /// This method processes records, performing retries as needed.
        /// </summary>
        /// <param name="records">The records to be processed.</param>
        protected void ProcessReceivedRecords(List<Record> records)
        {
            foreach (Record rec in records)
            {
                bool processedSuccessfully = false;
                TMessage data = null;

                try
                {
                    // Use the passed in serializer to interpret the message
                    data = serializer.Deserialize<TMessage>(rec.Data, rec.Data.Length);

                    // Your own logic to process a record goes here.
                    HandleMessage(data);
                    
                    processedSuccessfully = true;
                   
                }
                catch (Exception e)
                {
                    //Console.Error.WriteLine("Exception processing record data: " + data, e);

                    HandleProcessingFailure(rec, e);

                }

                if (!processedSuccessfully)
                {
                   //TODO: Decide what to do when message processing fails
                   //The answer is probably explode and die
                   // However the best way to do this is probably to just throw and exception from HandleProcessingFailure in the derived classes.
                }
            }
        }

        
        protected abstract void HandleMessage(TMessage message);
        protected abstract void HandleProcessingFailure(Record record, Exception exceptionEncountered);


        /// <summary>
        /// This checkpoints the specified checkpointer with retries.
        /// </summary>
        /// <param name="checkpointer">The checkpointer used to do checkpoints.</param>
        private void Checkpoint(Checkpointer checkpointer)
        {
            Console.Error.WriteLine("Checkpointing shard " + KinesisShardId);

            // You can optionally provide an error handling delegate to be invoked when checkpointing fails.
            // The library comes with a default implementation that retries for a number of times with a fixed
            // delay between each attempt. If you do not provide an error handler, the checkpointing operation
            // will not be retried, but processing will continue.
            checkpointer.Checkpoint(RetryingCheckpointErrorHandler.Create(NumOfRetries, Backoff));
        }

        public void LeaseLost(LeaseLossInput leaseLossInput)
        {
            //
            // Perform any necessary cleanup after losing your lease.  Checkpointing is not possible at this point.
            //
            Console.Error.WriteLine($"Lost lease on {KinesisShardId}");
        }

        public void ShardEnded(ShardEndedInput shardEndedInput)
        {
            //
            // Once the shard has ended it means you have processed all records on the shard. To confirm completion the
            // KCL requires that you checkpoint one final time using the default checkpoint value.
            //
            Console.Error.WriteLine(
                $"All records for {KinesisShardId} have been processed, starting final checkpoint");
            shardEndedInput.Checkpointer.Checkpoint();
        }

        public void ShutdownRequested(ShutdownRequestedInput shutdownRequestedInput)
        {
            Console.Error.WriteLine($"Shutdown has been requested for {KinesisShardId}. Checkpointing");
            shutdownRequestedInput.Checkpointer.Checkpoint();
        }
    }


}
