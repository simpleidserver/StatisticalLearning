﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Math.Entities
{
    public class NumberEntity : Entity
    {

        public NumberEntity(Number number)
        {
            Number = number;
        }

        public Number Number { get; set; }

        public override int CompareTo(object obj)
        {
            var source = obj as NumberEntity;
            if (source == null)
            {
                return -1;
            }

            return Number.Value.CompareTo(source.Number.Value);
        }


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