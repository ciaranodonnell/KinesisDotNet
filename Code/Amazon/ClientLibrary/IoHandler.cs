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
using System.IO;

namespace Amazon.Kinesis.ClientLibrary
{
    

    internal class IoHandler : IDisposable
    {
        private readonly StreamReader _reader;
        private readonly StreamWriter _outWriter;
        private readonly StreamWriter _errorWriter;

        public IoHandler()
            : this(Console.OpenStandardInput(), Console.OpenStandardOutput(), Console.OpenStandardError())
        {
        }

        public IoHandler(Stream inStream, Stream outStream, Stream errStream)
        {
            _reader = new StreamReader(inStream);
            _outWriter = new StreamWriter(outStream);
            _errorWriter = new StreamWriter(errStream);
        }

        public void WriteAction(Action a)
        {
            _outWriter.WriteLine(a.ToJson());
            _outWriter.Flush();
        }

        public Action ReadAction()
        {
            var s = _reader.ReadLine();
            return s == null ? null : Action.Parse(s);
        }

        public void WriteError(string message, Exception e)
        {
            _errorWriter.WriteLine(message);
            _errorWriter.WriteLine(e.StackTrace);
            _errorWriter.Flush();
        }

        public void Dispose()
        {
            _reader.Dispose();
            _outWriter.Dispose();
            _errorWriter.Dispose();
        }
    }
}