// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Math.Functions
{
    public class BetaFunction
    {

        public double Evaluate(double a, double b)
        {
            var gamma = new GammaFunction();
            var result = (gamma.EvaluateWithLanczos(a) * gamma.EvaluateWithLanczos(b)) / (gamma.EvaluateWithLanczos(a + b));
            return result;
        }
    }
}
