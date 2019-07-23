//
// Copyright 2019 Amazon.com, Inc. or its affiliates. All Rights Reserved.
//
// Licensed under the Amazon Software License (the "License").
// You may not use this file except in compliance with the License.
// A copy of the License is located at
//
//  http://aws.amazon.com/asl/
//
// or in the "license" file accompanying this file. This file is distributed
// on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied. See the License for the specific language governing
// permissions and limitations under the License.
//

namespace Amazon.Kinesis.ClientLibrary
{
    

    internal class ShardRecordProcessorToRecordProcessor : IShardRecordProcessor
    {
        private IRecordProcessor RecordProcessor { get; set; }

        internal ShardRecordProcessorToRecordProcessor(IRecordProcessor recordProcessor)
        {
            RecordProcessor = recordProcessor;
        }

        public void Initialize(InitializationInput input)
        {
            RecordProcessor.Initialize(input);
        }

        public void ProcessRecords(ProcessRecordsInput input)
        {
            RecordProcessor.ProcessRecords(input);
        }

        public void LeaseLost(LeaseLossInput leaseLossInput)
        {
            RecordProcessor.Shutdown(new DefaultShutdownInput(ShutdownReason.ZOMBIE, null));
        }

        public void ShardEnded(ShardEndedInput shardEndedInput)
        {
            RecordProcessor.Shutdown(new DefaultShutdownInput(ShutdownReason.TERMINATE, shardEndedInput.Checkpointer));
        }

        public void ShutdownRequested(ShutdownRequestedInput shutdownRequestedInput)
        {
            //
            // Does nothing
            //
        }
    }
}