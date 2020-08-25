// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using Xunit;

namespace StatisticalLearning.Tests.Math
{
    public class MatrixFixture
    {
        [Fact]
        public void When_Build_Identity_Matrix()
        {
            var result = Matrix.BuildIdentityMatrix(2);
            Assert.Equal(1, result.GetValue(0, 0).GetNumber());
            Assert.Equal(0, result.GetValue(0, 1).GetNumber());
            Assert.Equal(0, result.GetValue(1, 0).GetNumber());
            Assert.Equal(1, result.GetValue(1, 1).GetNumber());
        }

        [Fact]
        public void When_Multiply_Two_Matrix()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 1, 2, 3 },
                new double[] { 4, 5, 6 }
            };
            Matrix secondMatrix = new double[][]
            {
                new double[] { 7, 8 },
                new double[] { 9, 10 },
                new double[] { 11, 12 }
            };
            var result = firstMatrix.Multiply(secondMatrix);
            result.Evaluate();
            Assert.Equal(58, result.GetValue(0, 0).GetNumber());
            Assert.Equal(64, result.GetValue(0, 1).GetNumber());
            Assert.Equal(139, result.GetValue(1, 0).GetNumber());
            Assert.Equal(154, result.GetValue(1, 1).GetNumber());
        }

        [Fact]
        public void When_Substract_Two_Matrix()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 1, 2, 3 },
                new double[] { 4, 5, 6 }
            };
            Matrix secondMatrix = new double[][]
            {
                new double[] { 0, 1, 2 },
                new double[] { 3, 4, 5 },
            };
            var result = firstMatrix.Substract(secondMatrix);
            result.Evaluate();
            Assert.Equal(1, result.GetValue(0, 0).GetNumber());
            Assert.Equal(1, result.GetValue(0, 1).GetNumber());
            Assert.Equal(1, result.GetValue(0, 2).GetNumber());
            Assert.Equal(1, result.GetValue(1, 0).GetNumber());
            Assert.Equal(1, result.GetValue(1, 1).GetNumber());
            Assert.Equal(1, result.GetValue(1, 2).GetNumber());
        }

        [Fact]
        public void When_Transpose_Matrix()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 1, 2, 3 },
                new double[] { 4, 5, 6 }
            };
            var result = firstMatrix.Transpose();
            Assert.Equal(1, result.GetValue(0, 0).GetNumber());
            Assert.Equal(4, result.GetValue(0, 1).GetNumber());
            Assert.Equal(2, result.GetValue(1, 0).GetNumber());
            Assert.Equal(5, result.GetValue(1, 1).GetNumber());
            Assert.Equal(3, result.GetValue(2, 0).GetNumber());
            Assert.Equal(6, result.GetValue(2, 1).GetNumber());
        }

        [Fact]
        public void When_Calculate_Determinant()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 5, 4, 2 },
                new double[] { 1, 8, 2 },
                new double[] { 3, 1, 2 }
            };
            Matrix secondMatrix = new double[][]
            {
                new double[] { 1, 2, 3, 4 },
                new double[] { 5, 6, 7, 8 },
                new double[] { 9, 10, 11, 12 },
                new double[] { 13, 14, 15, 16 },
            };
            var firstDeterminant = firstMatrix.Determinant().Eval().GetNumber();
            var secondDeterminant = secondMatrix.Determinant().Eval().GetNumber();
            Assert.Equal(40, firstDeterminant);
            Assert.Equal(0, secondDeterminant);
        }

        [Fact]
        public void When_Inverse_Matrix()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 5, 10 },
                new double[] { 3, 8 }
            };
            Matrix secondMatrix = new double[][]
            {
                new double[] { 5, 4, 2 },
                new double[] { 1, 8, 2 },
                new double[] { 3, 1, 2 }
            };
            var firstResult = firstMatrix.Inverse();
            var secondResult = secondMatrix.Inverse();
            Assert.Equal(0.8, firstResult.GetValue(0, 0).GetNumber());
            Assert.Equal(-1, firstResult.GetValue(0, 1).GetNumber());
            Assert.Equal(0.35, System.Math.Round(secondResult.GetValue(0, 0).GetNumber(), 2));
            Assert.Equal(-0.15, System.Math.Round(secondResult.GetValue(0, 1).GetNumber(), 2));
            Assert.Equal(-0.2, System.Math.Round(secondResult.GetValue(0, 2).GetNumber(), 1));
        }

        [Fact]
        public void When_Calculate_Centered_Mean_Matrix()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 5.1, 3.5, 1.4 },
                new double[] { 4.9, 3.0, 1.4 },
                new double[] { 4.7, 3.2, 1.3 },
                new double[] { 4.6, 3.1, 1.5 },
                new double[] { 5.0, 3.6, 1.4 },
                new double[] { 7.0, 3.2, 4.7 },
                new double[] { 6.4, 3.2, 4.5 },
                new double[] { 6.9, 3.1, 4.9 },
                new double[] { 5.5, 2.3, 4.0 },
                new double[] { 6.5, 2.8, 4.6 },
                new double[] { 6.3, 3.3, 6.0 },
                new double[] { 5.8, 2.7, 5.1 },
                new double[] { 7.1, 3.0, 5.9 },
                new double[] { 6.3, 2.9, 5.6 },
                new double[] { 6.5, 3.0, 5.8 }
            };
            var result = firstMatrix.CenteredMeanMatrix();
            result.Evaluate();
            Assert.Equal(-0.81, System.Math.Round(result.GetValue(0, 0).GetNumber(), 2));
            Assert.Equal(0.44, System.Math.Round(result.GetValue(0, 1).GetNumber(), 2));
        }

        [Fact]
        public void When_Calculate_Centered_Reduced_Mean_Matrix()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 90, 140, 6.0 },
                new double[] { 60, 85, 5.9 },
                new double[] { 75, 135, 6.1 },
                new double[] { 70, 145, 5.8 },
                new double[] { 85, 130, 5.4 },
                new double[] { 70, 145, 5.0 }
            };
            var result = firstMatrix.CenteredMeanReducedMatrix(false);
            result.Evaluate();
            Assert.Equal(1.5, System.Math.Round(result.GetValue(0, 0).GetNumber(), 1));
            Assert.Equal(0.48, System.Math.Round(result.GetValue(0, 1).GetNumber(), 2));
        }

        [Fact]
        public void When_Calculate_Centered_Normative_Matrix()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 5.1, 3.5, 1.4 },
                new double[] { 4.9, 3.0, 1.4 },
                new double[] { 4.7, 3.2, 1.3 },
                new double[] { 4.6, 3.1, 1.5 },
                new double[] { 5.0, 3.6, 1.4 },
                new double[] { 7.0, 3.2, 4.7 },
                new double[] { 6.4, 3.2, 4.5 },
                new double[] { 6.9, 3.1, 4.9 },
                new double[] { 5.5, 2.3, 4.0 },
                new double[] { 6.5, 2.8, 4.6 },
                new double[] { 6.3, 3.3, 6.0 },
                new double[] { 5.8, 2.7, 5.1 },
                new double[] { 7.1, 3.0, 5.9 },
                new double[] { 6.3, 2.9, 5.6 },
                new double[] { 6.5, 3.0, 5.8 }
            };
            var result = firstMatrix.CenteredNormativeMatrix(false);
            result.Evaluate();
            Assert.Equal(-0.25, System.Math.Round(result.GetValue(0, 0).GetNumber(), 2));
            Assert.Equal(0.37, System.Math.Round(result.GetValue(0, 1).GetNumber(), 2));
        }

        [Fact]
        public void When_Calculate_Covariance()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 5, 2 },
                new double[] { 12, 8 },
                new double[] { 18, 18 },
                new double[] { 23, 20 },
                new double[] { 45, 28 }
            };
            var result = firstMatrix.Covariance();
            result.Evaluate();
            Assert.Equal(185.04, result.GetValue(0, 0).GetNumber());
            Assert.Equal(116.88, result.GetValue(0, 1).GetNumber());
        }

        [Fact]
        public void When_Calculate_Correlation()
        {
            Matrix firstMatrix = new double[][]
            {
                new double[] { 5, 2 },
                new double[] { 12, 8 },
                new double[] { 18, 18 },
                new double[] { 23, 20 },
                new double[] { 45, 28 }
            };
            var result = firstMatrix.Correlation(false);
            result.Evaluate();
            Assert.Equal(1, result.GetValue(0, 0).GetNumber());
            Assert.Equal(0.94, System.Math.Round(result.GetValue(0, 1).GetNumber(), 2));
        }
    }
}