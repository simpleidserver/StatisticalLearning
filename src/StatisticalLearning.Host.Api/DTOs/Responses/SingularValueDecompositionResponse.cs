// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Api.Host.DTOs.Responses
{
    public class SingularValueDecompositionResponse
    {
        public double[][] U { get; set; }
        public double[][] S { get; set; }
        public double[][] V { get; set; }
    }
}
