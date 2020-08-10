// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace StatisticalLearning.Api.Host.DTOs.Responses
{
    public class CoefficientResponse
    {
        public double Value { get; set; }
        public double StandardError { get; set; }
        public double TStatistic { get; set; }
        public double PValue { get; set; }

    }
}