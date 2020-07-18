// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Numeric;
using System;
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

            return null;
        }

        public override Entity Eval()
        {
            VariableEntity va = null, vb = null;
            NumberEntity na = null, nb = null;
            var a = Children.ElementAt(0).Eval();
            var b = Children.ElementAt(1).Eval();
            Entity result = this;
            if ((a.IsNumberEntity(out na) && b.IsNumberEntity(out nb)))
            {
                return EvalNumber(na, nb);
            }

            if (Name == Constants.Operators.MUL && a.IsOperatorEntity(new[] { Constants.Operators.SUB, Constants.Operators.SUM }, out OperatorEntity oa) && b.IsOperatorEntity(new[] { Constants.Operators.SUB, Constants.Operators.SUM }, out OperatorEntity ob))
            {
                return EvalMultiplicationOperator(oa, ob);
            }

            if (Name == Constants.Operators.MUL && (a.IsVariableEntity(out va) || b.IsVariableEntity(out vb)) &&
                ((a.IsNumberEntity(out na) || b.IsNumberEntity(out nb))))
            {
                return EvalMultiplicationOperator(va == null ? vb : va, na == null ? nb : na);
            }

            if (a.IsVariableEntity(out va) && b.IsVariableEntity(out vb))
            {
                return EvalVariable(va, vb);
            }

            switch (Name)
            {
                case Constants.Operators.MUL:
                    return a * b;
                case Constants.Operators.SUM:
                    return a + b;
                case Constants.Operators.DIV:
                    return a / b;
                default:
                    return a - b;
            }
        }

        public override string ToString()
        {
            var a = Children.ElementAt(0);
            var b = Children.ElementAt(1);
            var o = Constants.MappingOperatorToSign[Name];
            return $"{a.ToString()} {o} {b.ToString()}";
        }

        private Entity EvalNumber(NumberEntity na, NumberEntity nb)
        {
            switch (Name)
            {
                case Constants.Operators.MUL:
                    return na.Number * nb.Number;
                case Constants.Operators.SUM:
                    return na.Number + nb.Number;
                case Constants.Operators.DIV:
                    return na.Number / nb.Number;
                default:
                    return na.Number - nb.Number;
            }
        }

        private Entity EvalMultiplicationOperator(OperatorEntity oa, OperatorEntity ob)
        {
            Func<OperatorEntity, Entity> resolveValue = (o) =>
            {
                if (o.Name == Constants.Operators.SUB)
                {
                    return Number.Create(-1) * o.Children.ElementAt(1);
                }

                return o.Children.ElementAt(1);
            };
            
            var result = (oa.Children.ElementAt(0) * ob.Children.ElementAt(0) +
                oa.Children.ElementAt(0) * resolveValue(ob) +
                resolveValue(oa) * ob.Children.ElementAt(0) +
                resolveValue(oa) * resolveValue(ob)).Eval();
            return result;
        }

        private Entity EvalMultiplicationOperator(VariableEntity va, NumberEntity ne)
        {
            return va.Mul(ne);
        }

        private Entity EvalVariable(VariableEntity va, VariableEntity vb)
        {
            switch(Name)
            {
                case Constants.Operators.MUL:
                    return va.Mul(vb);
                case Constants.Operators.SUM:
                    return va.Sum(vb);
                case Constants.Operators.DIV:
                    return va.Div(vb);
                default:
                    return va.Sub(vb);
            }
        }
    }
}
