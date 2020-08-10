// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.MatrixDecompositions;
using StatisticalLearning.Statistic.Probability.Repartition;
using System.Linq;

namespace StatisticalLearning.Statistic.Regression
{
    public class MultipleLinearRegression
    {
        public LinearRegressionResult Regress(double[][] inputs, double[] outputs)
        {
            var decomposition = new SingularValueDecomposition();
            var decompositionResult = decomposition.Decompose(inputs);
            var coefficients = decompositionResult.V.Multiply(decompositionResult.S.Inverse()).Multiply(decompositionResult.U.Transpose()).Multiply(outputs.Select(_ => (NumberEntity)Number.Create(_)).ToArray()).Eval().GetColumnVector(0);
            var coefs = coefficients.Select(_ => (_ as NumberEntity).Number.Value).ToArray();
            int n = inputs.Length;
            int k = inputs[0].Length;
            var result = new LinearRegressionResult();
            var avgOutputs = CalculateAvg(outputs);
            var ssOutputs = CalculateSumOfSquares(outputs, avgOutputs);
            var rss = GetResidualSumOfSquare(coefs, inputs, outputs);
            var mse = GetMeanSquareError(rss, n, k);
            var rse = GetResidualStandardError(mse);
            var matrixInput = new Matrix(inputs);
            matrixInput = matrixInput.AddColumn(Number.Create(1));
            var covarianceMatrix = matrixInput.Transpose().Multiply(matrixInput).Inverse().Multiply(Number.Create(mse)).Eval();
            for (int index = 0; index < coefficients.Length; index++)
            {
                var coefficient = (coefficients[index] as NumberEntity).Number.Value;
                var covariance = covarianceMatrix.GetRowVector(index)[index];
                var standardError = (MathEntity.Sqrt(covariance).Eval() as NumberEntity).Number.Value;
                var tStat = CalculateTStatistic(coefficient, standardError);
                var pValue = CalculatePValue(tStat, n, k);
                var coefficientResult = new CoefficientResult { Value = coefficient, StandardError = standardError, TStatistic = tStat, PValue = pValue };
                if (index == 0)
                {
                    result.Intercept = coefficientResult;
                }
                else
                {
                    result.SlopeLst.Add(coefficientResult);
                }
            }

            result.ResidualSumOfSquare = rss;
            result.MeanSquareError = mse;
            result.ResidualStandardError = rse;
            result.RSquare = CalculateRSquare(rss, ssOutputs);
            return result;
        }

        private static double CalculateAvg(double[] values)
        {
            double totalSum = 0;
            foreach (var value in values)
            {
                totalSum += value;
            }

            return totalSum / values.Count();
        }

        private static double CalculateSumOfSquares(double[] values, double avg)
        {
            double result = 0;
            foreach (var value in values)
            {
                result += System.Math.Pow((value - avg), 2);
            }

            return result;
        }

        private static double GetResidualSumOfSquare(double[] coefficients, double[][] inputs, double[] outputs)
        {
            double result = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                var input = inputs[i];
                var output = outputs[i];
                double computedY = coefficients[0];
                for (int y = 0; y < input.Length; y++)
                {
                    computedY += coefficients[y + 1] * input[y];
                }

                result += System.Math.Pow(output - computedY, 2);
            }

            return result;
        }

        private static double GetMeanSquareError(double rse, int n, int p)
        {
            return rse / (n - p - 1); ;
        }

        private static double GetResidualStandardError(double mse)
        {
            return System.Math.Sqrt(mse);
        }

        private static double CalculateTStatistic(double estimatedCoefficient, double se)
        {
            return estimatedCoefficient / (se);
        }

        private static double CalculatePValue(double tStatistic, int n, int k)
        {
            int degreeOfFreedom = n - k - 1;
            var studentLaw = new StudentLaw();
            return studentLaw.ComputeUpperCumulative(tStatistic, degreeOfFreedom) * 2;
        }

        private static double CalculateRSquare(double rss, double tss)
        {
            return 1 - (rss / tss);
        }
    }
}
