// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalLearning.Statistic.Probability.Classifier
{
    public class GaussianNaiveBayes
    {
        public Matrix Sigma { get; set; }
        public Matrix Theta { get; set; }
        public Dictionary<Entity, double> ClassPriors { get; set; }

        public GaussianNaiveBayes Estimate(Matrix input, Vector output)
        {
            // Algorithm used to calculate variance + mean : http://i.stanford.edu/pub/cstr/reports/cs/tr/79/773/CS-TR-79-773.pdf
            var distinctOutput = output.Distinct();
            var nbFeatures = input.NbColumns;
            var nbClasses = distinctOutput.Count();
            var classCount = new int[nbClasses];
            var classPrior = new int[nbClasses];
            var epsilon = 1e-9 * input.Variance().Max().Key.GetNumber();
            Theta = Matrix.BuildEmptyMatrix(nbClasses, nbFeatures);
            Sigma = Matrix.BuildEmptyMatrix(nbClasses, nbFeatures);
            for(int i = 0; i < nbClasses; i++)
            {
                var ot = distinctOutput.Values[i];
                var indexes = output.FindIndexes(ot);
                Matrix subMatrix = indexes.Select(_ => input.GetRowVector(_).Values).ToArray();
                var kvp = UpdateMeanVariance(subMatrix);
                Theta.SetRow(kvp.Key, i);
                Sigma.SetRow(kvp.Value, i);
                classCount[i] = subMatrix.NbRows;
            }

            Sigma = Sigma.Sum(epsilon);
            CalculateClassPrior(output);
            return this;
        }

        public Vector Predict(Matrix input)
        {
            var probabilities = PredictProbability(input);
            var result = new Vector(input.NbRows);
            for(int i = 0; i < input.NbRows; i++)
            {
                var probs = probabilities.GetRowVector(i);
                var kvp = probs.Max();
                result[i] = ClassPriors.ElementAt(kvp.Value).Key;
            }

            return result;
        }

        public Matrix PredictProbability(Matrix input)
        {
            var jointLogLikelihood = new Vector[ClassPriors.Count()];
            for(int i = 0; i < ClassPriors.Count(); i++)
            {
                var kvp = ClassPriors.ElementAt(i);
                var jointi = System.Math.Log(kvp.Value);
                var nij = Sigma.GetRowVector(i).Multiply(System.Math.PI).Multiply(2.0).Log().Sum().GetNumber() * -0.5;
                var res = input.Substract(Theta.GetRowVector(i)).Pow(2).Div(Sigma.GetRowVector(i)).SumAllRows().Multiply(-0.5).Sum(nij);
                jointLogLikelihood[i] = res.Sum(jointi);
            }

            Matrix jointLogLikelihoodMatrix = jointLogLikelihood;
            jointLogLikelihoodMatrix = jointLogLikelihoodMatrix.Transpose();
            var logProbX = jointLogLikelihoodMatrix.Logsumexp();
            return jointLogLikelihoodMatrix.Substract(logProbX).Exp();
        }

        private KeyValuePair<Vector, Vector> UpdateMeanVariance(Matrix matrix)
        {
            var avg = matrix.Avg();
            var variance = matrix.Variance();
            return new KeyValuePair<Vector, Vector>(avg, variance);
        }

        private void CalculateClassPrior(Vector output)
        {
            ClassPriors = new Dictionary<Entity, double>();
            var nb = output.Length;
            foreach(var o in output.Distinct().Values)
            {
                double probability = (double)output.FindIndexes(o).Length / (double)nb;
                ClassPriors.Add(o, probability);
            }
        }
    }
}
