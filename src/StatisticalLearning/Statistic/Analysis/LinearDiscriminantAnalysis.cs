// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.MatrixDecompositions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace StatisticalLearning.Statistic.Analysis
{
    public class LinearDiscriminantAnalysis
    {
        private int _nbClasses;

        public LinearDiscriminantAnalysis()
        {
            _nbClasses = 2;
        }

        public LinearDiscriminantAnalysisResult Result { get; private set; }

        public LinearDiscriminantAnalysis Compute(Matrix input, Vector output)
        {
            var distinct = output.Distinct();
            Matrix sw = Matrix.BuildEmptyMatrix(input.NbColumns, input.NbColumns); // Within-class scatter matrix.
            Matrix sb = Matrix.BuildEmptyMatrix(input.NbColumns, input.NbColumns); // Between-class scatter matrix.
            var overallAvg = input.Avg();
            for (int i = 0; i < distinct.Count(); i++)
            {
                var cl = distinct[i];
                var indexes = output.FindIndexes(cl);
                Matrix subMatrix = indexes.Select(_ => input.GetRowVector(_).Values).ToArray();
                var avg = subMatrix.Avg();
                var d = avg.Substract(overallAvg);
                sb = sb.Sum(Matrix.Multiply(d, d).Multiply(subMatrix.NbRows)).Evaluate();
                var classBtMat = Matrix.BuildEmptyMatrix(input.NbColumns, input.NbColumns);
                var scatterMatrix = subMatrix.Scatter(avg).Evaluate();
                sw = sw.Sum(scatterMatrix).Evaluate();
            }

            var swsb = sw.Inverse().Multiply(sb).Evaluate();
            var decompositionResult = EigenvalueDecomposition.Decompose(swsb);
            var eingenVectors = decompositionResult.EigenVectors.Transpose();
            var eingenValues = decompositionResult.EigenValues;
            var dic = new Dictionary<Entity, Vector>();
            for (int i = 0; i < eingenValues.Length; i++)
            {
                var eingenValue = eingenValues[i];
                dic.Add(eingenValue, eingenVectors.GetRowVector(i));
            }

            var ordered = dic.OrderByDescending(_ => _.Key);
            double sumEingenValues = eingenValues.Values.Sum(_ => _.GetNumber());
            Matrix eigenVectorMatrix = Matrix.BuildEmptyMatrix(eingenVectors.NbRows, _nbClasses);           
            Result = new LinearDiscriminantAnalysisResult
            {
                ScatterBetweenClass = sb,
                ScatterWithinClass = sw
            };
            for (int i = 0; i < _nbClasses; i++)
            {
                var kvp = ordered.ElementAt(i);
                double variance = kvp.Key.GetNumber() / sumEingenValues;
                eigenVectorMatrix.SetColumn(kvp.Value, i);
                Result.Classes.Add(new LinearDiscriminantClass
                {
                    Variance = variance,
                    EingenValue = kvp.Key.GetNumber(),
                    EingenVector = kvp.Value.GetNumbers()
                });
            }

            Result.EigenVectorMatrix = eigenVectorMatrix;
            return this;
        }

        public Matrix Transform(Matrix input)
        {
            return input.Multiply(Result.EigenVectorMatrix).Evaluate();
        }
    }
}
