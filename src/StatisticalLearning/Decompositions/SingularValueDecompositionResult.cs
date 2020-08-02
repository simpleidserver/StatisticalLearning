// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using StatisticalLearning.Math;

namespace StatisticalLearning.Decompositions
{
    public class SingularValueDecompositionResult
    {
        public Matrix U { get; set; }
        public Matrix S { get; set; }
        public Matrix V { get; set; }
    }
}
