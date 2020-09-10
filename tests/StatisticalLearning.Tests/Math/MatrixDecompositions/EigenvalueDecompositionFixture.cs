// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using StatisticalLearning.Math;
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class EigenvalueDecompositionFixture
    {
        [Fact]
        public void When_Decompose_Matrix_With_EigenvalueDecomposition()
        {
            Matrix matrix = new double[][]
            {
                new double [] { -3.0247,   1.0485,  -8.0187,  -3.4249 },
                new double [] { -5.6209,   2.1525, -15.1084,  -6.3827 },
                new double [] {  8.029 ,  -2.8654,  21.3687,   9.0984 },
                new double [] { 10.6672,  -3.4133,  27.9905,  12.0531 }
            };
            var result = EigenvalueDecomposition.Decompose(matrix, false);
            Assert.Equal(32.27192, System.Math.Round(result.EigenValues[0].GetNumber(), 5));
        }
    }
}
