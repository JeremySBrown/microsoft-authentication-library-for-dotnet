﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Client.Mats.Internal
{
    internal class WamAction
    {
        public WamAction(string actionId, Scenario scenario)
        {
            ActionId = actionId;
            Scenario = scenario;
        }

        public string ActionId { get; }
        public Scenario Scenario { get; }
    }
}