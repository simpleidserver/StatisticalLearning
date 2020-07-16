// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Numeric;

namespace StatisticalLearning.Entities
{
    public class NumberEntity : Entity
    {

        public NumberEntity(Number number)
        {
            Number = number;
        }

        public Number Number { get; set; }

        public override Entity Derive()
        {
            return new NumberEntity(new Number(0));
        }

        public override Entity Eval()
        {
            return Number;
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
