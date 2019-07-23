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
    

    internal class DefaultKclProcess : KclProcess
    {
        private class InternalCheckpointer : Checkpointer
        {
            private readonly DefaultKclProcess _kclProcess;

            public InternalCheckpointer(DefaultKclProcess kclProcess)
            {
                _kclProcess = kclProcess;
            }

            internal override void Checkpoint(string sequenceNumber, CheckpointErrorHandler errorHandler = null)
            {
                _kclProcess._iohandler.WriteAction(new CheckpointAction(sequenceNumber));
                var response = _kclProcess._iohandler.ReadAction();
                if (response is CheckpointAction checkpointResponse)
                {
                    if (!string.IsNullOrEmpty(checkpointResponse.Error))
                    {
                        errorHandler?.Invoke(sequenceNumber, checkpointResponse.Error, this);
                    }
                }
                else
                {
                    errorHandler?.Invoke(sequenceNumber, $"Unexpected response type {response.GetType().Name}", this);
                }
            }
        }

        private readonly IShardRecordProcessor _processor;
        private readonly IoHandler _iohandler;
        private readonly Checkpointer _checkpointer;

        internal DefaultKclProcess(IShardRecordProcessor processor, IoHandler iohandler)
        {
            _processor = processor;
            _iohandler = iohandler;
            _checkpointer = new InternalCheckpointer(this);
        }

        public override void Run()
        {
            while (ProcessNextLine())
            {
            }
        }

        private bool ProcessNextLine()
        {
            Action a = _iohandler.ReadAction();
            if (a != null)
            {
                DispatchAction(a);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DispatchAction(Action action)
        {
            action.Dispatch(_processor, _checkpointer);
            _iohandler.WriteAction(new StatusAction(action.GetType()));
        }
    }
}