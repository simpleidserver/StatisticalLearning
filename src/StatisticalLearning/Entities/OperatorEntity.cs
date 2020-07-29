// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Numeric;
using System;
using System.Collections.Generic;
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
            OperatorEntity oa = null, ob = null;
            var a = Children.ElementAt(0).Eval();
            var b = Children.ElementAt(1).Eval();
            Entity result = this;
            if ((a.IsNumberEntity(out na) && b.IsNumberEntity(out nb)))
            {
                return EvalNumber(na, nb);
            }

            if (a.IsVariableEntity(out va) && b.IsVariableEntity(out vb))
            {
                return EvalVariable(Name, va, vb);
            }

            if (Name == Constants.Operators.MUL)
            {
                return EvalMultiplicationOperator(a, b);
            }

            if (Name == Constants.Operators.SUM || Name == Constants.Operators.SUB)
            {
                return EvalSumOrSubOperator(Name, a, b);
            }

            switch (Name)
            {
                case Constants.Operators.MUL:
                    return a * b;
                case Constants.Operators.DIV:
                    return a / b;
            }

            return null;
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

        private static Entity EvalMultiplicationOperator(Entity oa, Entity ob)
        {
            NumberEntity na = null, nb = null;
            VariableEntity va = null, vb = null;
            if ( (oa.IsVariableEntity(out va) || ob.IsVariableEntity(out vb)) &&
                (oa.IsNumberEntity(out na) || ob.IsNumberEntity(out nb)) )
            {
                return EvalMultiplicationOperator(va == null ? vb : va, na == null ? nb : na);
            }

            var oaChildren = GetSumOrSubEntities(oa);
            var obChildren = GetSumOrSubEntities(ob);
            Entity result = null;
            foreach(var oaChild in oaChildren)
            {
                foreach(var obChild in obChildren)
                {
                    if(result == null)
                    {
                        result = oaChild * obChild;
                    }
                    else
                    {
                        result = result + (oaChild * obChild);
                    }
                }
            }

            return result.Eval();
        }

        private static Entity EvalMultiplicationOperator(VariableEntity va, NumberEntity ne)
        {
            return va.Mul(ne);
        }

        private static Entity EvalSumOrSubOperator(string name, Entity a, Entity b)
        {
            VariableEntity va = null, vb = null;
            NumberEntity na = null, nb = null;
            OperatorEntity oa = null, ob = null;
            // (x + x2) + 3
            // (x + x2) + x5
            if ((a.IsOperatorEntity(new string[] { Constants.Operators.SUM, Constants.Operators.SUB }, out oa) || b.IsOperatorEntity(new string[] { Constants.Operators.SUM, Constants.Operators.SUB }, out ob)) &&
                ((a.IsVariableEntity(out va) || b.IsVariableEntity(out vb)) ||
                ((a.IsNumberEntity(out na) || b.IsNumberEntity(out nb)))))
            {
                var children = new List<Entity>();
                if (ob != null)
                {
                    children.Add(a);
                    if (name == Constants.Operators.SUB)
                    {
                        ob = (Number.Create(-1) * ob).Eval() as OperatorEntity;
                    }

                    var entities = GetSumOrSubEntities(ob);
                    children.AddRange(GetSumOrSubEntities(ob));
                }
                else
                {
                    children.AddRange(GetSumOrSubEntities(oa));
                    if (name == Constants.Operators.SUB)
                    {
                        b = (Number.Create(-1) * b).Eval();
                    }

                    children.Add(b);
                }

                var variables = children.Where(_ => _ is VariableEntity).GroupBy(_ => ((VariableEntity)_).Name + ((VariableEntity)_).Pow);
                var numbers = children.Where(_ => _ is NumberEntity);
                Entity variableEntity = null;
                foreach (var grp in variables)
                {
                    Entity subVariableEntity = null;
                    foreach (var variable in grp)
                    {
                        if (subVariableEntity == null)
                        {
                            subVariableEntity = variable;
                        }
                        else
                        {
                            subVariableEntity = (subVariableEntity + variable).Eval();
                        }
                    }

                    if (variableEntity == null)
                    {
                        variableEntity = subVariableEntity;
                    }
                    else
                    {
                        variableEntity = variableEntity + subVariableEntity;
                    }
                }

                Entity numberEntity = Number.Create(0);
                foreach (var number in numbers)
                {
                    numberEntity = (numberEntity + number).Eval();
                }

                return variableEntity + numberEntity;
            }

            // a == 0 or b == 0
            bool isAEmpty = false, isBEmpty = false;
            if (a.IsNumberEntity(out na) && (isAEmpty= na.Number.Value == 0) || b.IsNumberEntity(out nb) && (isBEmpty = nb.Number.Value == 0))
            {
                return isAEmpty ? b : a;
            }

            switch(name)
            {
                case Constants.Operators.SUM:
                    return a + b;
                default:
                    return a - b;
            }
        }

        private static Entity EvalVariable(string name, VariableEntity va, VariableEntity vb)
        {
            switch(name)
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
