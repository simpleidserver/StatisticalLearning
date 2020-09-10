// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Api.Host.DTOs.Responses
{
    public class LinearDiscriminantAnalysisResponse
    {
        public double[][] Transformed { get; set; }
        public LinearDiscriminantClassResponse[] LinearDiscriminantClasses { get; set; }
    }
}
