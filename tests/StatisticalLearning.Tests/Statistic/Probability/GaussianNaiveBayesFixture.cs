// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Statistic.Probability.Classifier;
using Xunit;

namespace StatisticalLearning.Tests.Statistic.Probability
{
    public class GaussianNaiveBayesFixture
    {
        [Fact]
        public void When_Calculate_Gaussian_Naive_Bayes_Distribution_Number()
        {
            double[][] inputs =
            {
                new double [] { 0, 1 },
                new double [] { 0, 2 },
                new double [] { 0, 1 },
                new double [] { 1, 2 },
                new double [] { 0, 2 },
                new double [] { 0, 2 },
                new double [] { 1, 1 },
                new double [] { 0, 1 },
                new double [] { 1, 1 }
            };
            double[][] predict =
            {
                new double[] { 0, 1 }
            };
            double[] outputs = // those are the class labels
            {
                0, 0, 0, 1, 1, 1, 2, 2, 2,
            };
            var bayes = new GaussianNaiveBayes();
            bayes.Estimate(inputs, outputs);
            var result = bayes.PredictProbability(predict);
            var prediction = bayes.Predict(predict);
            var firstRow = result.GetRowVector(0);
            Assert.Equal(0.68, System.Math.Round(firstRow.Values[0].GetNumber(), 2));
            Assert.Equal(0, firstRow.Values[1].GetNumber());
            Assert.Equal(0.32, System.Math.Round(firstRow.Values[2].GetNumber(), 2));
            Assert.Equal(1, prediction.Length);
            Assert.Equal(0, prediction.Values[0].GetNumber());
        }
    }
}
