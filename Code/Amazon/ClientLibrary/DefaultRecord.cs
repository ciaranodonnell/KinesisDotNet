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
using System.Runtime.Serialization;
using System.Text;

namespace Amazon.Kinesis.ClientLibrary
{
    

    [DataContract]
    internal class DefaultRecord : Record
    {
        [DataMember(Name = "sequenceNumber")] private string _sequenceNumber;

        [DataMember(Name = "subSequenceNumber")]
        private long? _subSequenceNumber;

        [DataMember(Name = "data")] private string _base64;
        private byte[] _data;

        [DataMember(Name = "partitionKey")] private string _partitionKey;

        [DataMember(Name = "approximateArrivalTimestamp")]
        private double _approximateArrivalTimestamp;

        public override string PartitionKey => _partitionKey;

        public override double ApproximateArrivalTimestamp => _approximateArrivalTimestamp;

        public override string SequenceNumber => _sequenceNumber;
        public override long? SubSequenceNumber => _subSequenceNumber;

        public override byte[] Data
        {
            get
            {
                if (_data != null)
                {
                    return _data;
                }

                _data = Convert.FromBase64String(_base64);
                _base64 = null;
                return _data;
            }
        }

        public DefaultRecord(string sequenceNumber, string partitionKey, string data, long? subSequenceNumber = null,
            double approximateArrivalTimestamp = 0d)
        {
            _data = Encoding.UTF8.GetBytes(data);
            _sequenceNumber = sequenceNumber;
            _partitionKey = partitionKey;
            _subSequenceNumber = subSequenceNumber;
            _approximateArrivalTimestamp = approximateArrivalTimestamp;
        }
    }
}