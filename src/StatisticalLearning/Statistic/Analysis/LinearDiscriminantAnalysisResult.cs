// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using System.Collections.Generic;

namespace StatisticalLearning.Statistic.Analysis
{
    public class LinearDiscriminantClass
    {   
        public double Variance { get; set; }
        public double EingenValue { get; set; }
        public double[] EingenVector { get; set; }
    }

    public class LinearDiscriminantAnalysisResult
    {
        public LinearDiscriminantAnalysisResult()
        {
            Classes = new List<LinearDiscriminantClass>();
        }

        public ICollection<LinearDiscriminantClass> Classes { get; set; }
        public Matrix ScatterBetweenClass { get; set; }
        public Matrix ScatterWithinClass { get; set; }
        public Matrix EigenVectorMatrix { get; set; }
    }
}
