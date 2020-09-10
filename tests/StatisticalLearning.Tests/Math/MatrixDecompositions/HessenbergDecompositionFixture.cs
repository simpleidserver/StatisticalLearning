// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class HessenbergDecompositionFixture
    {
        [Fact]
        public void When_Reduce_Matrix_To_Hessenberg_Form()
        {
            Matrix matrix = new double[][]
            {
                new double[] { 5.4, 4.0, 7.7 },
                new double[] { 3.5, -0.7, 2.8 },
                new double[] { -3.2, 5.1, 0.8 }
            };
            var result = HessenbergDecomposition.Decompose(matrix);
            Assert.Equal(-0.74, System.Math.Round(result.P.GetValue(1, 1).GetNumber(), 2));
            Assert.Equal(0.67, System.Math.Round(result.P.GetValue(1, 2).GetNumber(), 2));
            Assert.Equal(5.4, System.Math.Round(result.H.GetValue(0, 0).GetNumber(), 2));
            Assert.Equal(2.24, System.Math.Round(result.H.GetValue(0, 1).GetNumber(), 2));
            Assert.Equal(8.38, System.Math.Round(result.H.GetValue(0, 2).GetNumber(), 2));
        }
    }
}
