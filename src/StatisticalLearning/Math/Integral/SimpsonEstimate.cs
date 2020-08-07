// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Entities;
using System.Linq;

namespace StatisticalLearning.Math.Integral
{
    public class SimpsonEstimate
    {
        public double Estimate(Entity entity, VariableEntity variable, int n, double a, double b)
        {
            double nbIntervals = (b - a) / n;
            double sum = 0;
            int nb = 0;
            double x; 
            var nbDecimals = (b - System.Math.Truncate(b)).ToString().Split('.').Last().Count();
            if (nbDecimals > 15)
            {
                nbDecimals = 15;
            }

            for (x = a; System.Math.Round(x, nbDecimals) <= b; x += nbIntervals)
            {
                entity.Assign(variable, new NumberEntity(Number.Create(x)));
                var number = (entity.Eval() as NumberEntity).Number.Value;
                var nextValue = System.Math.Round(x + nbIntervals, nbDecimals);
                if (x != a && nextValue <= b)
                {
                    if (nb % 2 == 0)
                    {
                        number = 4 * number;
                    }
                    else
                    {
                        number = 2 * number;
                    }

                    nb++;
                }

                sum += number;
            }

            return (nbIntervals / 3) * sum;
        }
    }
}
