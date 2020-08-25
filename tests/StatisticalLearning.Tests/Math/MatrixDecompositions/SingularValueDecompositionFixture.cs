// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class SingularValueDecompositionFixture
    {
        [Fact]
        public void When_Decompose_SVD_Naive_Matrix()
        {
            Matrix matrix = new double[][]
            {
                new double[] { 3, 2, 2 },
                new double[] { 2, 3, -2 }
            };
            Matrix secondMatrix = new double[][]
            {
                new double[] { 4, 0 },
                new double[] { 3, -5 }
            };
            Matrix thirdMatrix = new double[][]
            {
                new double[] { 1, 80 },
                new double[] { 1, 60 },
                new double[] { 1, 10 },
                new double[] { 1, 20 },
                new double[] { 1, 30 }
            };
            Matrix fourthMatrix = new double[][]
            {
                new double[] { 1, 2.75, 5.3 },
                new double[] { 1, 2.5, 5.3 },
                new double[] { 1, 2.5, 5.3 }
            };
            var decomposition = new SingularValueDecomposition();
            var result = decomposition.DecomposeNaive(matrix).Result;
            var secondResult = decomposition.DecomposeNaive(secondMatrix).Result;
            var thirdResult = decomposition.DecomposeNaive(thirdMatrix).Result;
            var fourthResult = decomposition.DecomposeNaive(fourthMatrix).Result;
            var exceptedMatrix = result.U.Multiply(result.S).Multiply(result.V.Transpose()).Evaluate();
            var secondExceptedMatrix = secondResult.U.Multiply(secondResult.S).Multiply(secondResult.V.Transpose()).Evaluate();
            var thirdExceptedMatrix = thirdResult.U.Multiply(thirdResult.S).Multiply(thirdResult.V.Transpose()).Evaluate();
            var fourthExceptedMatrix = fourthResult.U.Multiply(fourthResult.S).Multiply(fourthResult.V.Transpose()).Evaluate();
            Assert.Equal(3, System.Math.Round(exceptedMatrix.GetValue(0, 0).GetNumber()));
            Assert.Equal(2, System.Math.Round(exceptedMatrix.GetValue(0, 1).GetNumber()));
            Assert.Equal(2, System.Math.Round(exceptedMatrix.GetValue(0, 2).GetNumber()));
            Assert.Equal(4, System.Math.Round(secondExceptedMatrix.GetValue(0, 0).GetNumber()));
            Assert.Equal(0, System.Math.Round(secondExceptedMatrix.GetValue(0, 1).GetNumber()));
            Assert.Equal(1, System.Math.Round(thirdExceptedMatrix.GetValue(0, 0).GetNumber()));
            Assert.Equal(80, System.Math.Round(thirdExceptedMatrix.GetValue(0, 1).GetNumber()));
            Assert.Equal(1, System.Math.Round(fourthExceptedMatrix.GetValue(0, 0).GetNumber()));
            Assert.Equal(2.75, System.Math.Round(fourthExceptedMatrix.GetValue(0, 1).GetNumber(), 2));
            Assert.Equal(5.3, System.Math.Round(fourthExceptedMatrix.GetValue(0, 2).GetNumber(), 1));
        }

        [Fact]
        public void When_Decompose_SVD_GolubReinsch_Matrix()
        {
            Matrix matrix = new double[][]
            {
                new double[] { 4, 0 },
                new double[] { 3, -5 }
            };
            var decomposition = new SingularValueDecomposition();
            var result = decomposition.DecomposeGolubReinsch(matrix).Result;
            var exceptedMatrix = result.U.Multiply(result.S).Multiply(result.V.Transpose()).Evaluate();
            Assert.Equal(4, System.Math.Round(exceptedMatrix.GetValue(0, 0).GetNumber()));
            Assert.Equal(0, System.Math.Round(exceptedMatrix.GetValue(0, 1).GetNumber()));
            Assert.Equal(3, System.Math.Round(exceptedMatrix.GetValue(1, 0).GetNumber()));
            Assert.Equal(-5, System.Math.Round(exceptedMatrix.GetValue(1, 1).GetNumber()));
        }
    }
}
