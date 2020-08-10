// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Api.Host.DTOs.Requests
{
    public class GetNormalLawRequest
    {
        public double Average { get; set; }
        public double StandardDeviation { get; set; }
        public double Value { get; set; }
    }
}
