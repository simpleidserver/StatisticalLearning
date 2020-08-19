// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using System;
using System.Linq;
using Xunit;

namespace StatisticalLearning.Tests.Math
{
    public class MatrixFixture
    {
        [Fact]
        public void When_Build_Identity_Matrix()
        {
            var result = Matrix.BuildIdentityMatrix(2);
            var exceptedMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(0) },
                new Entity[] { Number.Create(0), Number.Create(1) }
            });
            Assert.True(result.Equals(exceptedMatrix));
        }

        [Fact]
        public void When_Multiply_Two_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(2), Number.Create(3) },
                new Entity[] { Number.Create(4), Number.Create(5), Number.Create(6) },
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(7), Number.Create(8) },
                new Entity[] { Number.Create(9), Number.Create(10) },
                new Entity[] { Number.Create(11), Number.Create(12) },
            });
            var exceptedMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(58), Number.Create(64) },
                new Entity[] { Number.Create(139), Number.Create(154) }
            });
            var result = firstMatrix.Multiply(secondMatrix);
            result.Solve();
            Assert.True(result.Equals(exceptedMatrix));
        }

        [Fact]
        public void When_Substract_Two_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(2), Number.Create(3) },
                new Entity[] { Number.Create(4), Number.Create(5), Number.Create(6) },
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(0), Number.Create(1), Number.Create(2) },
                new Entity[] { Number.Create(3), Number.Create(4), Number.Create(5) },
            });
            var exceptedMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(1), Number.Create(1) },
                new Entity[] { Number.Create(1), Number.Create(1), Number.Create(1) }
            });
            var result = firstMatrix.Substract(secondMatrix);
            result.Solve();
            Assert.True(result.Equals(exceptedMatrix));
        }

        [Fact]
        public void When_Transpose_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(2), Number.Create(3) },
                new Entity[] { Number.Create(4), Number.Create(5), Number.Create(6) }
            });
            var exceptedMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(4) },
                new Entity[] { Number.Create(2), Number.Create(5) },
                new Entity[] { Number.Create(3), Number.Create(6) }
            });
            var result = firstMatrix.Transpose();
            var secondResult = result.Transpose();
            Assert.True(result.Equals(exceptedMatrix));
            Assert.True(secondResult.Equals(firstMatrix));
        }

        [Fact]
        public void When_Compute_Determinant()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(5), Number.Create(4), Number.Create(2) },
                new Entity[] { Number.Create(1), Number.Create(8), Number.Create(2) },
                new Entity[] { Number.Create(3), Number.Create(1), Number.Create(2) }
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(2), Number.Create(3), Number.Create(4) },
                new Entity[] { Number.Create(5), Number.Create(6), Number.Create(7), Number.Create(8) },
                new Entity[] { Number.Create(9), Number.Create(10), Number.Create(11), Number.Create(12) },
                new Entity[] { Number.Create(13), Number.Create(14), Number.Create(15), Number.Create(16) },
            });

            var firstDeterminant = firstMatrix.ComputeDeterminant().Eval() as NumberEntity;
            var secondDeterminant = secondMatrix.ComputeDeterminant().Eval() as NumberEntity;
            Assert.NotNull(firstDeterminant);
            Assert.NotNull(secondDeterminant);
            Assert.Equal(40, firstDeterminant.Number.Value);
            Assert.Equal(0, secondDeterminant.Number.Value);
        }

        [Fact]
        public void When_Inverse_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(5), Number.Create(10) },
                new Entity[] { Number.Create(3), Number.Create(8) }
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(5), Number.Create(4), Number.Create(2) },
                new Entity[] { Number.Create(1), Number.Create(8), Number.Create(2) },
                new Entity[] { Number.Create(3), Number.Create(1), Number.Create(2) }
            });
            var firstResult = firstMatrix.Inverse();
            var secondResult = secondMatrix.Inverse();
            Assert.Equal("[ [ 1,0 ],[ 0,1 ] ]", firstMatrix.Multiply(firstResult).Eval().ToString());
            Assert.Equal("[ [ 0,35,-0,15,-0,2 ],[ 0,1,0,1,-0,2 ],[ -0,575,0,175,0,9 ] ]", secondResult.ToString());
        }

        [Fact]
        public void When_Compute_Centered_Mean_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(5.1), Number.Create(3.5), Number.Create(1.4) },
                new Entity[] { Number.Create(4.9), Number.Create(3.0), Number.Create(1.4) },
                new Entity[] { Number.Create(4.7), Number.Create(3.2), Number.Create(1.3) },
                new Entity[] { Number.Create(4.6), Number.Create(3.1), Number.Create(1.5) },
                new Entity[] { Number.Create(5.0), Number.Create(3.6), Number.Create(1.4) },
                new Entity[] { Number.Create(7.0), Number.Create(3.2), Number.Create(4.7) },
                new Entity[] { Number.Create(6.4), Number.Create(3.2), Number.Create(4.5) },
                new Entity[] { Number.Create(6.9), Number.Create(3.1), Number.Create(4.9) },
                new Entity[] { Number.Create(5.5), Number.Create(2.3), Number.Create(4.0) },
                new Entity[] { Number.Create(6.5), Number.Create(2.8), Number.Create(4.6) },
                new Entity[] { Number.Create(6.3), Number.Create(3.3), Number.Create(6.0) },
                new Entity[] { Number.Create(5.8), Number.Create(2.7), Number.Create(5.1) },
                new Entity[] { Number.Create(7.1), Number.Create(3.0), Number.Create(5.9) },
                new Entity[] { Number.Create(6.3), Number.Create(2.9), Number.Create(5.6) },
                new Entity[] { Number.Create(6.5), Number.Create(3.0), Number.Create(5.8) }
            });
            var result = firstMatrix.ComputeMeanCenteredMatrix();
            Assert.Equal("[ [ -0,806666666666666,0,44,-2,47333333333333 ],[ -1,00666666666667,-0,0600000000000001,-2,47333333333333 ],[ -1,20666666666667,0,14,-2,57333333333333 ],[ -1,30666666666667,0,04,-2,37333333333333 ],[ -0,906666666666665,0,54,-2,47333333333333 ],[ 1,09333333333333,0,14,0,826666666666667 ],[ 0,493333333333335,0,14,0,626666666666666 ],[ 0,993333333333335,0,04,1,02666666666667 ],[ -0,406666666666665,-0,76,0,126666666666666 ],[ 0,593333333333335,-0,26,0,726666666666666 ],[ 0,393333333333334,0,24,2,12666666666667 ],[ -0,106666666666666,-0,36,1,22666666666667 ],[ 1,19333333333333,-0,0600000000000001,2,02666666666667 ],[ 0,393333333333334,-0,16,1,72666666666667 ],[ 0,593333333333335,-0,0600000000000001,1,92666666666667 ] ]", result.ToString());
        }

        [Fact]
        public void When_Compute_Centered_Reduced_Mean_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(90), Number.Create(140), Number.Create(6.0) },
                new Entity[] { Number.Create(60), Number.Create(85), Number.Create(5.9) },
                new Entity[] { Number.Create(75), Number.Create(135), Number.Create(6.1) },
                new Entity[] { Number.Create(70), Number.Create(145), Number.Create(5.8) },
                new Entity[] { Number.Create(85), Number.Create(130), Number.Create(5.4) },
                new Entity[] { Number.Create(70), Number.Create(145), Number.Create(5.0) }
            });
            var result = firstMatrix.ComputeMeanCenteredReducedMatrix(false);
            Assert.Equal("[ [ 1,5,0,480384461415261,0,78334945180064 ],[ -1,5,-2,16173007636868,0,522232967867094 ],[ 0,0,240192230707631,1,04446593573419 ],[ -0,5,0,720576692122892,0,261116483933546 ],[ 1,0,-0,78334945180064 ],[ -0,5,0,720576692122892,-1,82781538753483 ] ]", result.ToString());
        }

        [Fact]
        public void When_Compute_Centered_Normative_Matrix()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(5.1), Number.Create(3.5), Number.Create(1.4) },
                new Entity[] { Number.Create(4.9), Number.Create(3.0), Number.Create(1.4) },
                new Entity[] { Number.Create(4.7), Number.Create(3.2), Number.Create(1.3) },
                new Entity[] { Number.Create(4.6), Number.Create(3.1), Number.Create(1.5) },
                new Entity[] { Number.Create(5.0), Number.Create(3.6), Number.Create(1.4) },
                new Entity[] { Number.Create(7.0), Number.Create(3.2), Number.Create(4.7) },
                new Entity[] { Number.Create(6.4), Number.Create(3.2), Number.Create(4.5) },
                new Entity[] { Number.Create(6.9), Number.Create(3.1), Number.Create(4.9) },
                new Entity[] { Number.Create(5.5), Number.Create(2.3), Number.Create(4.0) },
                new Entity[] { Number.Create(6.5), Number.Create(2.8), Number.Create(4.6) },
                new Entity[] { Number.Create(6.3), Number.Create(3.3), Number.Create(6.0) },
                new Entity[] { Number.Create(5.8), Number.Create(2.7), Number.Create(5.1) },
                new Entity[] { Number.Create(7.1), Number.Create(3.0), Number.Create(5.9) },
                new Entity[] { Number.Create(6.3), Number.Create(2.9), Number.Create(5.6) },
                new Entity[] { Number.Create(6.5), Number.Create(3.0), Number.Create(5.8) }
            });
            var result = firstMatrix.ComputeCenteredNormativeMatrix(false);
            Assert.Equal("[ [ -0,246728206130356,0,369760954999048,-0,349400351980143 ],[ -0,307900488642014,-0,0504219484089611,-0,349400351980143 ],[ -0,369072771153673,0,117651212954243,-0,363527050847265 ],[ -0,399658912409502,0,0336146322726407,-0,335273653113021 ],[ -0,277314347386185,0,45379753568065,-0,349400351980143 ],[ 0,3344084777304,0,117651212954243,0,116780710634873 ],[ 0,150891630195425,0,117651212954243,0,0885273129006292 ],[ 0,303822336474571,0,0336146322726407,0,145034108369116 ],[ -0,124383641107039,-0,638678013180174,0,0178938185650208 ],[ 0,181477771451254,-0,218495109772165,0,102654011767751 ],[ 0,120305488939595,0,201687793635844,0,300427795907455 ],[ -0,0326252173395509,-0,302531690453766,0,173287506103359 ],[ 0,36499461898623,-0,0504219484089611,0,286301097040333 ],[ 0,120305488939595,-0,134458529090563,0,243921000438968 ],[ 0,181477771451254,-0,0504219484089611,0,272174398173211 ] ]", result.ToString());
        }

        [Fact]
        public void When_Compute_Covariance()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(5), Number.Create(2) },
                new Entity[] { Number.Create(12), Number.Create(8) },
                new Entity[] { Number.Create(18), Number.Create(18) },
                new Entity[] { Number.Create(23), Number.Create(20) },
                new Entity[] { Number.Create(45), Number.Create(28) }
            });
            var result = firstMatrix.ComputeCovariance();
            Assert.Equal("[ [ 185,04,116,88 ],[ 116,88,84,16 ] ]", result.ToString());
        }

        [Fact]
        public void When_Compute_Correlation()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(5), Number.Create(2) },
                new Entity[] { Number.Create(12), Number.Create(8) },
                new Entity[] { Number.Create(18), Number.Create(18) },
                new Entity[] { Number.Create(23), Number.Create(20) },
                new Entity[] { Number.Create(45), Number.Create(28) }
            });
            var result = firstMatrix.ComputeCorrelation(false);
            Assert.Equal("[ [ 1,0,936600811300924 ],[ 0,936600811300924,1 ] ]", result.ToString());
        }
    }
}