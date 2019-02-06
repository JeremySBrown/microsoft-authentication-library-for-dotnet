//------------------------------------------------------------------------------
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
//------------------------------------------------------------------------------

using Android.Content;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Exceptions;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.UI;
using System;
using System.Threading;
using System.Threading.Tasks;
using Uri = System.Uri;

namespace Microsoft.Identity.Client.Platforms.Android.SystemWebview
{
    [global::Android.Runtime.Preserve(AllMembers = true)]
    internal class SystemWebUI : WebviewBase
    {
        private readonly CoreUIParent _parent;
        private readonly RequestContext _requestContext;

        public SystemWebUI(CoreUIParent parent, RequestContext requestContext)
        {
            _parent = parent;
            _requestContext = requestContext;
        }

        public override async Task<Uri> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ReturnedUriReady = new SemaphoreSlim(0);

            try
            {
                var agentIntent = new Intent(_parent.Activity, typeof(AuthenticationActivity));
                agentIntent.PutExtra(AndroidConstants.RequestUrlKey, authorizationUri.AbsoluteUri);
                agentIntent.PutExtra(AndroidConstants.CustomTabRedirect, redirectUri.OriginalString);
                AuthenticationActivity.RequestContext = _requestContext;
                _parent.Activity.RunOnUiThread(() => _parent.Activity.StartActivityForResult(agentIntent, 0));
            }
            catch (Exception ex)
            {
                _requestContext.Logger.ErrorPii(ex);
                throw MsalExceptionFactory.GetClientException(
                    CoreErrorCodes.AuthenticationUiFailedError,
                    "AuthenticationActivity failed to start",
                    ex);
            }

            await ReturnedUriReady.WaitAsync(cancellationToken).ConfigureAwait(false);
            return AuthCodeUri;
        }

        public override void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: true);
        }
    }
}