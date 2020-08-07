// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Math.Functions
{
    public class GammaFunction
    {
        private const int G = 7;
        private readonly double[] _lanczorscoefs = new double[]
        {
             0.99999999999980993,
            676.5203681218851,
            -1259.1392167224028,
             771.32342877765313,
            -176.61502916214059,
            12.507343278686905,
            -0.13857109526572012,
             9.9843695780195716e-6,
             1.5056327351493116e-7
        };

        public double EvaluateWithLanczos(double a)
        {
            // https://en.wikipedia.org/wiki/Lanczos_approximation
            if (a < 0.5)
            {
                return System.Math.PI / (System.Math.Sin(System.Math.PI * a) * EvaluateWithLanczos(1 - a));
            }

            a -= 1;
            var x = _lanczorscoefs[0];
            for(int i = 1; i <= G + 1; i++)
            {
                x += _lanczorscoefs[i] / (a + i);
            }

            var t = a + G + 0.5;
            return System.Math.Sqrt(2 * System.Math.PI) * System.Math.Pow(t, a + 0.5) * System.Math.Pow(System.Math.E, -t) * x;
        }
    }
}
