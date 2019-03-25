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
using Microsoft.Identity.Client.Cache;

namespace Microsoft.Identity.Client
{
    /// <summary>
    /// Notification for certain token cache interactions during token acquisition. This delegate is
    /// used in particular to provide a custom token cache serialization.
    /// See https://aka.ms/aka.ms/msal-net-token-cache-serialization
    /// </summary>
    /// <param name="args">Arguments related to the cache item impacted</param>
    public delegate void TokenCacheCallback(TokenCacheNotificationArgs args);

    /// <summary>
    /// This is the interface that implements the public access to cache operations.
    /// With CacheV2, this should only be necessary if the caller is persisting
    /// the cache in their own store, since this will provide the serialize/deserialize
    /// and before/after notifications used in that scenario.
    /// See https://aka.ms/aka.ms/msal-net-token-cache-serialization
    /// </summary>
    public interface ITokenCache
    {
#if !ANDROID_BUILDTIME && !iOS_BUILDTIME 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beforeAccess"></param>
        void SetBeforeAccess(TokenCacheCallback beforeAccess);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afterAccess"></param>
        void SetAfterAccess(TokenCacheCallback afterAccess);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beforeWrite"></param>
        void SetBeforeWrite(TokenCacheCallback beforeWrite);

        /// <summary>
        /// Serializes the token cache to the MSAL.NET 3.x cache format, which is compatible with other MSAL desktop libraries, e.g. MSAL for Python and MSAL for Java.
        /// If you need to maintain SSO between an application using ADAL 3.x or MSAL 2.x and this application using MSAL 3.x,
        /// you might also want to serialize and deserialize with <see cref="SerializeAdalV3"/>/<see cref="DeserializeAdalV3"/> or <see cref="SerializeMsalV2"/>/<see cref="DeserializeMsalV2"/>, 
        /// otherwise just use <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/>.
        /// </summary>
        /// <returns>Byte stream representation of the cache</returns>
        /// <remarks>
        /// This is the recommended format for maintaining SSO state between applications.
        /// <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/> is compatible with other MSAL libraries such as MSAL for Python and MSAL for Java.
        /// </remarks>
        byte[] SerializeMsalV3();

        /// <summary>
        /// Deserializes the token cache to the MSAL.NET 3.x cache format, which is compatible with other MSAL desktop libraries, e.g. MSAL for Python and MSAL for Java.
        /// If you need to maintain SSO between an application using ADAL 3.x or MSAL 2.x and this application using MSAL 3.x,
        /// you might also want to serialize and deserialize with <see cref="SerializeAdalV3"/>/<see cref="DeserializeAdalV3"/> or <see cref="SerializeMsalV2"/>/<see cref="DeserializeMsalV2"/>, 
        /// otherwise just use <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/>.
        /// </summary>
        /// <param name="msalV3State">Byte stream representation of the cache</param>
        /// <remarks>
        /// This is the recommended format for maintaining SSO state between applications.
        /// <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/> is compatible with other MSAL libraries such as MSAL for Python and MSAL for Java.
        /// </remarks>
        void DeserializeMsalV3(byte[] msalV3State);

        /// <summary>
        /// Serializes the token cache to the MSAL.NET 2.x unified cache format, which is compatible with ADAL.NET v4 and other MSAL.NET v2 applications.
        /// If you need to maintain SSO between an application using ADAL 3.x or MSAL 2.x and this application using MSAL 3.x,
        /// you might also want to serialize and deserialize with <see cref="SerializeAdalV3"/>/<see cref="DeserializeAdalV3"/> or <see cref="SerializeMsalV2"/>/<see cref="DeserializeMsalV2"/>, 
        /// otherwise just use <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/>.
        /// </summary>
        /// <returns>Byte stream representation of the cache</returns>
        /// <remarks>
        /// <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/> is compatible with other MSAL libraries such as MSAL for Python and MSAL for Java.
        /// </remarks>
        byte[] SerializeMsalV2();

        /// <summary>
        /// Deserializes the token cache to the MSAL.NET 2.x cache format, which is compatible with ADAL.NET v4 and other MSAL.NET v2 applications.
        /// If you need to maintain SSO between an application using ADAL 3.x or MSAL 2.x and this application using MSAL 3.x,
        /// you might also want to serialize and deserialize with <see cref="SerializeAdalV3"/>/<see cref="DeserializeAdalV3"/> or <see cref="SerializeMsalV2"/>/<see cref="DeserializeMsalV2"/>, 
        /// otherwise just use <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/>.
        /// </summary>
        /// <param name="msalV2State">Byte stream representation of the cache</param>
        /// <remarks>
        /// <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/> is compatible with other MSAL libraries such as MSAL for Python and MSAL for Java.
        /// </remarks>
        void DeserializeMsalV2(byte[] msalV2State);

        /// <summary>
        /// Serializes the token cache to the ADAL.NET 3.x cache format.
        /// If you need to maintain SSO between an application using ADAL 3.x or MSAL 2.x and this application using MSAL 3.x,
        /// you might also want to serialize and deserialize with <see cref="SerializeAdalV3"/>/<see cref="DeserializeAdalV3"/> or <see cref="SerializeMsalV2"/>/<see cref="DeserializeMsalV2"/>, 
        /// otherwise just use <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/>.
        /// </summary>
        /// <returns>Byte stream representation of the cache</returns>
        /// <remarks>
        /// <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/> is compatible with other MSAL libraries such as MSAL for Python and MSAL for Java.
        /// </remarks>
        byte[] SerializeAdalV3();

        /// <summary>
        /// Deserializes the token cache to the ADAL.NET 3.x cache format.
        /// If you need to maintain SSO between an application using ADAL 3.x or MSAL 2.x and this application using MSAL 3.x,
        /// you might also want to serialize and deserialize with <see cref="SerializeAdalV3"/>/<see cref="DeserializeAdalV3"/> or <see cref="SerializeMsalV2"/>/<see cref="DeserializeMsalV2"/>, 
        /// otherwise just use <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/>.
        /// </summary>
        /// <param name="adalV3State">Byte stream representation of the cache</param>
        /// <remarks>
        /// <see cref="SerializeMsalV3"/>/<see cref="DeserializeMsalV3"/> is compatible with other MSAL libraries such as MSAL for Python and MSAL for Java.
        /// </remarks>
        void DeserializeAdalV3(byte[] adalV3State);

        /// <summary>
        /// Functionality replaced by <see cref="SerializeMsalV2"/>. See https://aka.ms/msal-net-3x-cache-breaking-change
        /// </summary>
        /// <returns></returns>
        [Obsolete("This is expected to be removed in MSAL.NET v3 and ADAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
        byte[] Serialize();

        /// <summary>
        /// Functionality replaced by <see cref="DeserializeMsalV2"/>.  See https://aka.ms/msal-net-3x-cache-breaking-change
        /// </summary>
        /// <param name="msalV2State"></param>
        [Obsolete("This is expected to be removed in MSAL.NET v3 and ADAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
        void Deserialize(byte[] msalV2State);

        /// <summary>
        /// Functionality replaced by <see cref="SerializeMsalV2"/> and <see cref="SerializeAdalV3"/>
        /// See https://aka.ms/msal-net-3x-cache-breaking-change
        /// </summary>
        /// <returns></returns>
        [Obsolete("This is expected to be removed in MSAL.NET v3 and ADAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
        CacheData SerializeUnifiedAndAdalCache();

        /// <summary>
        /// Functionality replaced by <see cref="DeserializeMsalV2"/> and <see cref="DeserializeAdalV3"/>
        /// See https://aka.ms/msal-net-3x-cache-breaking-change
        /// </summary>
        /// <param name="cacheData"></param>
        [Obsolete("This is expected to be removed in MSAL.NET v3 and ADAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
        void DeserializeUnifiedAndAdalCache(CacheData cacheData);
#endif
    }
}