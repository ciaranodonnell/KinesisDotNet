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

using System;
using System.Collections.Generic;
using System.Threading;

namespace Amazon.Kinesis.ClientLibrary
{
    
    /// <summary>
    /// Instances of this delegate can be passed to Checkpointer's Checkpoint methods. The delegate will be
    /// invoked when a checkpoint operation fails.
    /// </summary>
    /// <param name="sequenceNumber">The sequence number at which the checkpoint was attempted.</param>
    /// <param name="errorMessage">The error message received from the checkpoint failure.</param>
    /// <param name="checkpointer">The Checkpointer instance that was used to perform the checkpoint operation.</param>
    public delegate void CheckpointErrorHandler(string sequenceNumber, string errorMessage, Checkpointer checkpointer);

  

}