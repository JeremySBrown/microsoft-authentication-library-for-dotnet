﻿// ------------------------------------------------------------------------------
// 
// Copyright (c) Microsoft Corporation.
// All rights reserved.
// 
// This code is licensed under the MIT License.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ------------------------------------------------------------------------------

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonCache.Test.Common;

namespace CommonCache.Test.MsalJava
{
    public class JavaLanguageExecutor : ILanguageExecutor
    {
        public JavaLanguageExecutor(string javaClassPath)
        {
            JavaClassPath = javaClassPath;
        }

        // Path to java class with a public static void main() function to execute
        public string JavaClassPath { get; }

        public async Task<ProcessRunResults> ExecuteAsync(
            string clientId,
            string authority,
            string scope,
            string username,
            string password,
            string cacheFilePath,
            CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            sb.Append($"{JavaClassPath.EncloseQuotes()} ");
            sb.Append($"{clientId} ");
            sb.Append($"{authority} ");
            sb.Append($"{scope} ");
            sb.Append($"{username} ");
            sb.Append($"{password} ");
            sb.Append($"{cacheFilePath.EncloseQuotes()} ");
            string arguments = sb.ToString();

            string executablePath = "java.exe";

            Console.WriteLine($"Calling:  {executablePath} {arguments}");

            var processUtils = new ProcessUtils();

            var processRunResults = await processUtils.RunProcessAsync(executablePath, arguments, cancellationToken).ConfigureAwait(false);
            return processRunResults;
        }
    }
}
