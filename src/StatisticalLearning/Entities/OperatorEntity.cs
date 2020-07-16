// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System.Linq;

namespace StatisticalLearning.Entities
{
    public class OperatorEntity : Entity
    {
        public OperatorEntity(string name, int value = 0)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public int Value { get; set; }

        public override Entity Derive()
        {
            switch(Name)
            {
                // (a * b)' = a' * b + b' * a
                case Constants.Operators.MUL:
                    {
                        var a = Children.ElementAt(0);
                        var b = Children.ElementAt(1);
                        return a.Derive() * b + b.Derive() * a;
                    }
                // (a + b)' = a' + b'
                case Constants.Operators.SUM:
                    {
                        var a = Children.ElementAt(0);
                        var b = Children.ElementAt(1);
                        return a.Derive() + b.Derive();
                    }
            }

            return base.Derive();
        }

        public override Entity Eval()
        {
            var a = Children.ElementAt(0).Eval();
            var b = Children.ElementAt(1).Eval();
            Entity result = this;
            if (a.IsNumberEntity(out NumberEntity na) && b.IsNumberEntity(out NumberEntity nb))
            {
                switch (Name)
                {
                    case Constants.Operators.MUL:
                        result = na.Number * nb.Number;
                        break;
                    case Constants.Operators.SUM:
                        result = na.Number + nb.Number;
                        break;
                    case Constants.Operators.DIV:
                        result = na.Number / nb.Number;
                        break;
                    case Constants.Operators.SUB:
                        result = na.Number - nb.Number;
                        break;
                }
            }
            else
            {
                switch (Name)
                {
                    case Constants.Operators.MUL:
                        result = a * b;
                        break;
                    case Constants.Operators.SUM:
                        result = a + b;
                        break;
                    case Constants.Operators.DIV:
                        result = a / b;
                        break;
                    case Constants.Operators.SUB:
                        result = a - b;
                        break;
                }
            }

            return result;
        }

        public override string ToString()
        {
            var a = Children.ElementAt(0);
            var b = Children.ElementAt(1);
            var o = Constants.MappingOperatorToSign[Name];
            return $"{a.ToString()} {o} {b.ToString()}";
        }
    }
}
