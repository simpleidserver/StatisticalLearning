// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class QRDecompositionFixture
    {
        [Fact]
        public void When_Decompose_With_QR()
        {
            Matrix matrix = new double[][]
            {
                new double[] { 1, 2, 2, 6, 4 },
                new double[] { 5, 4, 8, 9, 7 },
                new double[] { 5, 3, 9, 7, 8 },
                new double[] { 2, 1, 1, 5, 5 },
                new double[] { 2, 4, 7, 7, 9 }
            };
            var result = QRDecomposition.Decompose(matrix);
            Assert.Equal(2.5567, System.Math.Round(result.R.GetValue(4, 4).GetNumber(), 4));
            Assert.Equal(2.2847, System.Math.Round(result.R.GetValue(3, 4).GetNumber(), 4));
            Assert.Equal(-0.4053, System.Math.Round(result.R.GetValue(2, 4).GetNumber(), 4));
            Assert.Equal(5.3878, System.Math.Round(result.R.GetValue(1, 4).GetNumber(), 4));
            Assert.Equal(13.9302, System.Math.Round(result.R.GetValue(0, 4).GetNumber(), 4));
        }
    }
}
