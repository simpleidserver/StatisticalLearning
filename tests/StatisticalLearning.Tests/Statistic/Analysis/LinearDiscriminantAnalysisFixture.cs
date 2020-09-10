// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Statistic.Analysis;
using System.IO;
using System.Linq;
using Xunit;

namespace StatisticalLearning.Tests.Statistic.Analysis
{
    public class LinearDiscriminantAnalysisFixture
    {
        [Fact]
        public void When_Calculate_LinearDiscriminantAnalysis_IRIS_Data()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "iris.data");
            Matrix input = File.ReadAllLines(path).Select(_ => _.Split(',').ToArray()).ToArray();
            Matrix x = input.GetSubMatrix(0, 0, input.NbColumns - 1).Values.Select(_ => _.Select(__ => double.Parse(__.ToString().Replace('.', ','))).ToArray()).ToArray();
            Vector y = input.GetSubMatrix(0, input.NbColumns - 1).GetColumnVector(0);
            var linearDiscriminantAnalysis = new LinearDiscriminantAnalysis();
            linearDiscriminantAnalysis.Compute(x, y);
            Assert.Equal(39, System.Math.Round(linearDiscriminantAnalysis.Result.ScatterWithinClass.GetValue(0, 0).GetNumber()));
            Assert.Equal(63, System.Math.Round(linearDiscriminantAnalysis.Result.ScatterBetweenClass.GetValue(0, 0).GetNumber()));
        }
    }
}
