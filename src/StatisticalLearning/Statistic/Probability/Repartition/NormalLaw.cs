// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Math.Entities;
using StatisticalLearning.Math.Integral;

namespace StatisticalLearning.Statistic.Probability.Repartition
{
    public class NormalLaw
    {
        private const string _avgVariableName = "µ";
        private const string _standardDeviationVariableName = "σ";
        private const string _variableName = "x";
        private int _n;
        private int _a;
        
        public NormalLaw()
        {
            _n = 200;
            _a = -100;
        }

        public double ComputeProbability(double average, double standardDeviation, double value)
        {
            VariableEntity µ = _avgVariableName;
            VariableEntity σ = _standardDeviationVariableName;
            VariableEntity x = _variableName;
            var exponent = Number.Create(-0.5) * MathEntity.Pow((x - µ) / σ, Number.Create(2));
            var integralEquation = MathEntity.Pow(Number.Create(System.Math.E), exponent);
            integralEquation.Assign(µ, new NumberEntity(Number.Create(average)));
            integralEquation.Assign(σ, new NumberEntity(Number.Create(standardDeviation)));
            var estimate = new SimpsonEstimate();
            var estimation = estimate.Estimate(integralEquation, x, _n, _a, value);
            var equation = (Number.Create(1) / (σ * MathEntity.Sqrt(Number.Create(2) * Number.Create(System.Math.PI)))) * Number.Create(estimation);
            equation.Assign(σ, new NumberEntity(Number.Create(standardDeviation)));
            return (equation.Eval() as NumberEntity).Number.Value;
        }

        public Entity GetEquation()
        {
            VariableEntity µ = _avgVariableName;
            VariableEntity σ = _standardDeviationVariableName;
            VariableEntity x = _variableName;
            var firstEntity = Number.Create(1) / (σ * MathEntity.Sqrt(Number.Create(2) * Number.Create(System.Math.PI)));
            var secondEntity = (Number.Create(-1) * MathEntity.Pow(x - µ, Number.Create(2))) / (Number.Create(2) * MathEntity.Pow(σ, Number.Create(2)));
            return MathEntity.Pow(firstEntity, secondEntity);
        }
    }
}
