// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Numeric
{
    public class Number
    {
        public Number(double value)
        {
            Value = value;
        }

        public double Value { get; private set; }

        public static Number Create(double number)
        {
            return new Number(number);
        }

        public static implicit operator Number(double num) => new Number(num);

        public static Number operator *(Number a, Number b) => new Number(a.Value * b.Value);
        public static Number operator /(Number a, Number b) => new Number(a.Value / b.Value);
        public static Number operator +(Number a, Number b) => new Number(a.Value + b.Value);
        public static Number operator -(Number a, Number b) => new Number(a.Value - b.Value);

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
