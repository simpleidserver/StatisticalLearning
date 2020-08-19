// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Extensions
{
    public static class CollectionExtensions
    {
        public static double ComputeMean(this double[] inputs)
        {
            return inputs.ComputeSum() / inputs.Length;
        }

        public static double ComputeStandardDeviation(this double[] inputs, bool unbiased = true)
        {
            var mean = inputs.ComputeMean();
            double sum = 0;
            foreach(var input in inputs)
            {
                sum += System.Math.Pow(input - mean, 2);
            }

            if (unbiased)
            {
                return System.Math.Sqrt(sum / (inputs.Length - 1));
            }

            return System.Math.Sqrt(sum / (inputs.Length));
        }

        public static double ComputeSum(this double[] inputs)
        {
            double sum = 0;
            foreach (var input in inputs)
            {
                sum += input;
            }

            return sum;
        }
    }
}
