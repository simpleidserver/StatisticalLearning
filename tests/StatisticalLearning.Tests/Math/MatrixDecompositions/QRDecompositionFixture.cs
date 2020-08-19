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
            var matrix = new Matrix(new double[][]
            {
                new double[] { 1, -1, 4 },
                new double[] { 1, 4, -2 },
                new double[] { 1, 4, 2 },
                new double[] { 1, -1, 0 }
            });
            var qrDecomposition = new QRDecomposition();
            var result = qrDecomposition.Decompose(matrix);
            Assert.Equal("[ [ 0,5,-0,5,0,5 ],[ 0,5,0,5,-0,5 ],[ 0,5,0,5,0,5 ],[ 0,5,-0,5,-0,5 ] ]", result.Q.ToString());
            Assert.Equal("[ [ 2,3,2 ],[ 0,5,-2 ],[ 0,0,4 ] ]", result.R.ToString());
        }
    }
}
