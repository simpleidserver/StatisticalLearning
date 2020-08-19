﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.MatrixEchelons;
using Xunit;

namespace StatisticalLearning.Tests.Math.MatrixEchelons
{
    public class RowEchelonFormFixture
    {
        [Fact]
        public void When_Build_Row_Echelon_Form()
        {
            var firstMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(0), Number.Create(2) },
                new Entity[] { Number.Create(1), Number.Create(0) }
            });
            var secondMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(8), Number.Create(8) },
                new Entity[] { Number.Create(8), Number.Create(8) }
            });
            var thirdMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(-12), Number.Create(12), Number.Create(2) },
                new Entity[] { Number.Create(12), Number.Create(-12), Number.Create(-2) },
                new Entity[] { Number.Create(2), Number.Create(-2), Number.Create(-17) },
            });
            var fourthMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(13), Number.Create(12), Number.Create(2) },
                new Entity[] { Number.Create(12), Number.Create(13), Number.Create(-2) },
                new Entity[] { Number.Create(2), Number.Create(-2), Number.Create(8) }
            });
            var fifthMatrix = new Matrix(new Entity[][]
            {
                new Entity[] { Number.Create(-11398.5092), Number.Create(200) },
                new Entity[] { Number.Create(200), Number.Create(-3.5092) }
            });
            var rowEchelon = new RowEchelon();
            var firstResult = rowEchelon.BuildReducedRowEchelonForm(firstMatrix);
            var secondResult = rowEchelon.BuildReducedRowEchelonForm(secondMatrix);
            var thirdResult = rowEchelon.BuildReducedRowEchelonForm(thirdMatrix);
            var fourthResult = rowEchelon.BuildReducedRowEchelonForm(fourthMatrix);
            var fifthResult = rowEchelon.BuildReducedRowEchelonForm(fifthMatrix);

            Assert.NotNull(firstResult);
            Assert.NotNull(secondResult);
            Assert.NotNull(thirdResult);
            Assert.Equal("[ [ 1,0 ],[ 0,1 ] ]", firstResult.ToString());
            Assert.Equal("[ [ 1,1 ],[ 0,0 ] ]", secondResult.ToString());
            Assert.Equal("[ [ 1,-1,0 ],[ 0,0,1 ],[ 0,0,0 ] ]", thirdResult.ToString());
            Assert.Equal("[ [ 1,0,2 ],[ 0,1,-2 ],[ 0,0,0 ] ]", fourthResult.ToString());
            Assert.Equal("[ [ 1,-0,0175461541935677 ],[ 0,0 ] ]", fifthResult.ToString());
        }
    }
}