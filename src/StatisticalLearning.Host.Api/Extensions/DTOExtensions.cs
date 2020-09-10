// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.MatrixDecompositions;
using StatisticalLearning.Statistic.Analysis;
using StatisticalLearning.Statistic.Regression;
using System.Linq;

namespace StatisticalLearning.Api.Host.Extensions
{
    public static class DTOExtensions
    {
        public static DTOs.Responses.LinearDiscriminantClassResponse ToDto(this LinearDiscriminantClass ld)
        {
            return new DTOs.Responses.LinearDiscriminantClassResponse
            {
                EigenValue = ld.EingenValue,
                Eigenvector = ld.EingenVector
            };
        }

        public static DTOs.Responses.PrincipalComponentResponse ToDto(this PrincipalComponent pc)
        {
            return new DTOs.Responses.PrincipalComponentResponse
            {
                Cumulative = pc.Cumulative.GetNumber(),
                EigenValue = pc.EigenValue.GetNumber(),
                Eigenvector = pc.Eigenvector.GetNumbers(),
                Proportion =  pc.Proportion.GetNumber(),
                SingularValue = pc.SingularValue.GetNumber()
            };
        }

        public static DTOs.Responses.LogisticRegressionResult ToDto(this LogisticRegression logisticRegression)
        {
            return new DTOs.Responses.LogisticRegressionResult
            {
                OddsRatio = logisticRegression.GetOddsRatio(),
                StandardErrors = logisticRegression.GetStandardErrors(),
                Regression = logisticRegression.LinearRegressionResult.ToDto()
            };
        }

        public static DTOs.Responses.LinearRegressionResponse ToDto(this LinearRegressionResult result)
        {
            return new DTOs.Responses.LinearRegressionResponse
            {
                Intercept = new DTOs.Responses.CoefficientResponse
                {
                    PValue = Number(result.Intercept.Value),
                    StandardError = result.Intercept.StandardError,
                    TStatistic = result.Intercept.TStatistic,
                    Value = result.Intercept.Value
                },
                Slope = result.SlopeLst.Select(_ =>
                    new DTOs.Responses.CoefficientResponse
                    {
                        PValue = Number(_.PValue),
                        StandardError = _.StandardError,
                        TStatistic = _.TStatistic,
                        Value = _.Value
                    }
                ).ToList(),
                ResidualStandardError = result.ResidualStandardError,
                ResidualSumOfSquares = result.ResidualSumOfSquare,
                RSquare = result.RSquare
            };
        }

        public static DTOs.Responses.SingularValueDecompositionResponse ToDto(this SingularValueDecompositionResult result)
        {
            return new DTOs.Responses.SingularValueDecompositionResponse
            {
                U = result.U.DoubleValues,
                S = result.S.DoubleValues,
                V = result.V.DoubleValues
            };
        }


        private static double Number(double number)
        {
            if (double.IsNaN(number))
            {
                return 0;
            }

            return number;
        }
    }
}
