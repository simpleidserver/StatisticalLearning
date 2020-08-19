// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Api.Host.DTOs.Responses
{
    public class PrincipalComponentResponse
    {
        public double Cumulative { get; set; }
        public double Proportion { get; set; }
        public double SingularValue { get; set; }
        public double EigenValue { get; set; }
        public double[] Eigenvector { get; set; }
    }
}
