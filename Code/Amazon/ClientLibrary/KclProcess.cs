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
    /// <summary>
    /// Instances of KclProcess communicate with the multi-lang daemon. The Main method of your application must
    /// create an instance of KclProcess and call its Run method.
    /// </summary>
    public abstract class KclProcess
    {
        /// <summary>
        /// Create an instance of KclProcess that uses the given IRecordProcessor to process records.
        /// </summary>
        /// <param name="recordProcessor">IRecordProcessor used to process records.</param>
        public static KclProcess Create(IRecordProcessor recordProcessor)
        {
            return Create(recordProcessor, new IoHandler());
        }

        public static KclProcess Create(IShardRecordProcessor recordProcessor)
        {
            return Create(recordProcessor, new IoHandler());
        }

        internal static KclProcess Create(IRecordProcessor recordProcessor, IoHandler ioHandler)
        {
            return new DefaultKclProcess(new ShardRecordProcessorToRecordProcessor(recordProcessor), ioHandler);
        }

        internal static KclProcess Create(IShardRecordProcessor recordProcessor, IoHandler ioHandler)
        {
            return new DefaultKclProcess(recordProcessor, ioHandler);
        }

        /// <summary>
        /// Starts the KclProcess. Once this method is called, the KclProcess instance will continuously communicate with
        /// the multi-lang daemon, performing actions as appropriate. This method blocks until it detects that the
        /// multi-lang daemon has terminated the communication.
        /// </summary>
        public abstract void Run();
    }
}