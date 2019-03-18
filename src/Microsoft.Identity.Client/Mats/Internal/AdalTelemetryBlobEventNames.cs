﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Client.Mats.Internal
{
    internal class AdalTelemetryBlobEventNames
    {
        public const string AdalCorrelationIdConstStrKey = "Microsoft.ADAL.correlation_id";
        public const string ApiIdConstStrKey = "Microsoft_ADAL_api_id";
        public const string BrokerAppConstStrKey = "Microsoft_ADAL_broker_app";
        public const string CacheEventCountConstStrKey = "Microsoft_ADAL_cache_event_count";
        public const string HttpEventCountTelemetryBatchKey = "Microsoft_ADAL_http_event_count";
        public const string IdpConstStrKey = "Microsoft_ADAL_idp";
        public const string IsSilentTelemetryBatchKey = "";
        public const string IsSuccessfulConstStrKey = "Microsoft_ADAL_is_successful";
        public const string ResponseTimeConstStrKey = "Microsoft_ADAL_response_time";
        public const string TenantIdConstStrKey = "Microsoft_ADAL_tenant_id";
        public const string UiEventCountTelemetryBatchKey = "Microsoft_ADAL_ui_event_count";
    }
}