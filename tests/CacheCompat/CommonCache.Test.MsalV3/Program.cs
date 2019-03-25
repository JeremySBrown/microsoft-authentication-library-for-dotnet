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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCache.Test.Common;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.AppConfig;

namespace CommonCache.Test.MsalV2
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new MsalV3CacheExecutor().Execute(args);
        }

        private class MsalV3CacheExecutor : AbstractCacheExecutor
        {
            /// <inheritdoc />
            protected override async Task<CacheExecutorResults> InternalExecuteAsync(CommandLineOptions options)
            {
                var v1App = PreRegisteredApps.CommonCacheTestV1;
                string resource = PreRegisteredApps.MsGraph;
                string[] scopes = new[]
                {
                    resource + "/user.read"
                };

                CommonCacheTestUtils.EnsureCacheFileDirectoryExists();

                var app = PublicClientApplicationBuilder
                    .Create(v1App.ClientId)
                    .WithAuthority(new Uri(v1App.Authority), true)
                    .WithLogging((LogLevel level, string message, bool containsPii) =>
                    {
                        Console.WriteLine("{0}: {1}", level, message);
                    })
                    .Build();

                FileBasedTokenCacheHelper.ConfigureUserCache(
                    options.CacheStorageType,
                    app.UserTokenCache,
                    CommonCacheTestUtils.AdalV3CacheFilePath,
                    CommonCacheTestUtils.MsalV2CacheFilePath,
                    CommonCacheTestUtils.MsalV3CacheFilePath);

                IEnumerable<IAccount> accounts = await app.GetAccountsAsync().ConfigureAwait(false);
                try
                {
                    var result = await app.AcquireTokenSilentAsync(scopes, accounts.FirstOrDefault(), app.Authority, false).ConfigureAwait(false);
                    Console.WriteLine($"got token for '{result.Account.Username}' from the cache");
                    return new CacheExecutorResults(result.Account.Username, true);
                }
                catch (MsalUiRequiredException)
                {
                    var result = await app.AcquireTokenByUsernamePasswordAsync(scopes, options.Username, options.UserPassword.ToSecureString()).ConfigureAwait(false);
                    Console.WriteLine($"got token for '{result.Account.Username}' without the cache");
                    return new CacheExecutorResults(result.Account.Username, false);
                }
            }
        }
    }
}
