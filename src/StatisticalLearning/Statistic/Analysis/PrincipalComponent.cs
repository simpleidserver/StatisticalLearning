// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;

namespace StatisticalLearning.Statistic.Analysis
{
    public class PrincipalComponent
    {
        public Entity Cumulative { get; set; }
        public Entity Proportion { get; set; }
        public Entity SingularValue { get; set; }
        public Entity EigenValue { get; set; }
        public Vector Eigenvector { get; set; }
    }
}
