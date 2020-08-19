// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.MatrixDecompositions;
using StatisticalLearning.Statistic.Analysis;
using StatisticalLearning.Statistic.Regression;
using System.Linq;

namespace StatisticalLearning.Api.Host.Extensions
{
    public static class DTOExtensions
    {
        public static DTOs.Responses.PrincipalComponentResponse ToDto(this PrincipalComponent pc)
        {
            return new DTOs.Responses.PrincipalComponentResponse
            {
                Cumulative = pc.Cumulative,
                EigenValue = pc.EigenValue,
                Eigenvector = pc.Eigenvector,
                Proportion =  pc.Proportion,
                SingularValue = pc.SingularValue
            };
        }

        public static DTOs.Responses.LinearRegressionResponse ToDto(this LinearRegressionResult result)
        {
            return new DTOs.Responses.LinearRegressionResponse
            {
                Intercept = new DTOs.Responses.CoefficientResponse
                {
                    PValue = result.Intercept.Value,
                    StandardError = result.Intercept.StandardError,
                    TStatistic = result.Intercept.TStatistic,
                    Value = result.Intercept.Value
                },
                Slope = result.SlopeLst.Select(_ =>
                    new DTOs.Responses.CoefficientResponse
                    {
                        PValue = _.PValue,
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
                U = result.U.DoubleArr,
                S = result.S.DoubleArr,
                V = result.V.DoubleArr
            };
        }
    }
}
