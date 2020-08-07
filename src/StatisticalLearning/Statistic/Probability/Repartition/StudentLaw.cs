// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.Functions;
using StatisticalLearning.Math.Integral;

namespace StatisticalLearning.Statistic.Probability.Repartition
{
    public class StudentLaw
    {
        private const string _variableName = "x";
        private const string _degreeOfFreedomName = "v";
        private int _n;
        private int _a;

        public StudentLaw()
        {
            _n = 200;
            _a = -100;
        }

        public double ComputeUpperCumulative(double tStat, double degreeOfFreedom)
        {
            var lower = ComputeLowerCumulative(tStat, degreeOfFreedom);
            return 1 - lower;
        }

        public double ComputeLowerCumulative(double tStat, double degreeOfFreedom)
        {
            VariableEntity x = _variableName;
            VariableEntity v = _degreeOfFreedomName;
            var betaFunction = new BetaFunction();
            var betaResult = betaFunction.Evaluate(0.5, degreeOfFreedom / 2);
            var firstEntity = MathEntity.Pow(Number.Create(1) + (MathEntity.Pow(x, Number.Create(2)) / v), Number.Create(-1) * ((v + Number.Create(1)) / Number.Create(2)));
            var secondEntity = MathEntity.Sqrt(v) * Number.Create(betaResult);
            var result = (firstEntity / secondEntity);
            result.Assign(v, (NumberEntity)Number.Create(degreeOfFreedom));
            var simpsonEstimate = new SimpsonEstimate();
            return simpsonEstimate.Estimate(result, x, _n, _a, tStat);
        }
    }
}
