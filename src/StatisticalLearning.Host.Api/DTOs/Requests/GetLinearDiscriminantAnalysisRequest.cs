// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Api.Host.DTOs.Requests
{
    public class GetLinearDiscriminantAnalysisRequest
    {
        public double[][] Input { get; set; }
        public string[] Output { get; set; }
    }
}
