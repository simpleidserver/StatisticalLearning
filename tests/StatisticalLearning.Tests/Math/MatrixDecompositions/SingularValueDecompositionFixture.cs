// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.MatrixDecompositions;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixDecompositions
{
    public class SingularValueDecompositionFixture
    {
        [Fact]
        public void When_Decompose_SVD_Matrix()
        {
            var matrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(3), Number.Create(2), Number.Create(2) },
                new Entity[] { Number.Create(2), Number.Create(3), Number.Create(-2) }
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(4), Number.Create(0) },
                new Entity[] { Number.Create(3), Number.Create(-5) }
            });
            var thirdMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(1), Number.Create(80) },
                new Entity[] { Number.Create(1), Number.Create(60) },
                new Entity[] { Number.Create(1), Number.Create(10) },
                new Entity[] { Number.Create(1), Number.Create(20) },
                new Entity[] { Number.Create(1), Number.Create(30) }
            });
            var fourthMatrix = new Matrix(new double[][]
            {
                new double[] { 1, 2.75, 5.3 },
                new double[] { 1, 2.5, 5.3 },
                new double[] { 1, 2.5, 5.3 }
            });
            var decomposition = new SingularValueDecomposition();
            var result = decomposition.Decompose(matrix);
            var secondResult = decomposition.Decompose(secondMatrix);
            var thirdResult = decomposition.Decompose(thirdMatrix);
            var fourthResult = decomposition.Decompose(fourthMatrix);
            var exceptedMatrix = result.U.Multiply(result.S).Multiply(result.V.Transpose()).Eval();
            var secondExceptedMatrix = secondResult.U.Multiply(secondResult.S).Multiply(secondResult.V.Transpose()).Eval();
            var thirdExceptedMatrix = thirdResult.U.Multiply(thirdResult.S).Multiply(thirdResult.V.Transpose()).Eval();
            var fourthExceptedMatrix = fourthResult.U.Multiply(fourthResult.S).Multiply(fourthResult.V.Transpose()).Eval();
            Assert.True(matrix.Equals(exceptedMatrix));
            Assert.True(secondMatrix.Equals(secondExceptedMatrix));
            Assert.Equal("[ [ 0,999999999999946,80 ],[ 0,999999999999959,60 ],[ 0,999999999999993,10 ],[ 0,999999999999986,20 ],[ 0,999999999999979,30 ] ]", thirdExceptedMatrix.ToString());
            Assert.Equal("[ [ 0,999999999997021,2,7499999999998,5,30000000000065 ],[ 1,00000000000246,2,4999999999998,5,29999999999963 ],[ 1,00000000000246,2,4999999999998,5,29999999999963 ] ]", fourthExceptedMatrix.ToString());
        }

        [Fact]
        public void When_Use_Golub_Reinsch_To_Calculate_SVD()
        {
            var matrix = new Matrix(new double[][]
            {
                new double[] { 4, 0 },
                new double[] { 3, -5 }
            });
            var decomposition = new SingularValueDecomposition();
            var result = decomposition.DecomposeGolubReinsch(matrix);
            var exceptedMatrix = result.U.Multiply(result.S).Multiply(result.V.Transpose()).Eval();
            Assert.Equal("[ [ 4,-6,66133814775094E-16 ],[ 3,-5 ] ]", exceptedMatrix.ToString());
        }
    }
}
