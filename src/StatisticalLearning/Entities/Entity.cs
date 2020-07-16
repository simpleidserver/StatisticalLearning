// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math;
using StatisticalLearning.Numeric;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalLearning.Entities
{
    public class Entity
    {
        public static implicit operator Entity(Number number) => new NumberEntity(number);
        public static implicit operator Entity(string name) => new VariableEntity(name);
        public static Entity operator +(Entity a, Entity b)
        {
            var result = new OperatorEntity(Constants.Operators.SUM);
            result.Children.Add(a);
            result.Children.Add(b);
            return result;
        }
        public static Entity operator -(Entity a, Entity b)
        {
            var result = new OperatorEntity(Constants.Operators.SUB);
            result.Children.Add(a);
            result.Children.Add(b);
            return result;
        }

        public static Entity operator *(Entity a, Entity b)
        {
            var result = new OperatorEntity(Constants.Operators.MUL);
            result.Children.Add(a);
            result.Children.Add(b);
            return result;
        }
        public static Entity operator /(Entity a, Entity b)
        {
            var result = new OperatorEntity(Constants.Operators.DIV);
            result.Children.Add(a);
            result.Children.Add(b);
            return result;
        }


        public Entity()
        {
            Children = new List<Entity>();
        }

        public ICollection<Entity> Children { get; private set; }

        public virtual Entity Derive()
        {
            return this;
        }

        public Entity Solve(VariableEntity variable)
        {
            var dic = new Dictionary<int, Entity>();
            var children = GetSumEntities(this);
            foreach (var child in children)
            {
                var monomial = ParseMonomial(child, variable);
                if (dic.ContainsKey(monomial.Key))
                {
                    dic[monomial.Key] += monomial.Value;
                }
                else
                {
                    dic.Add(monomial.Key, monomial.Value);
                }
            }

            var orders = dic.OrderBy(_ => _.Key);
            if (orders.Count() == 2)
            {
                var expr = (Number.Create(-1) * orders.ElementAt(orders.ElementAt(0).Key).Value) / orders.ElementAt(orders.ElementAt(1).Key).Value;
                var value = expr.Eval();
                var power = orders.ElementAt(1).Key;
                for (var i = 2; i <= power; i++)
                {
                    // value = System.Math.Sqrt(value);
                }

                return value;
            }

            return null;
        }

        public virtual Entity Eval()
        {
            return this;
        }

        public bool IsOperatorEntity(string type, out OperatorEntity result)
        {
            result = null;
            var r = this as OperatorEntity;
            if (r.Name == type)
            {
                result = r;
                return true;
            }

            return false;
        }

        public bool IsNumberEntity(out NumberEntity result)
        {
            result = this as NumberEntity;
            return result != null;                
        }

        private static KeyValuePair<int, Entity> ParseMonomial(Entity monomialExpr, VariableEntity variable)
        {
            int pow = 0;
            if (!monomialExpr.Children.Any())
            {
                return new KeyValuePair<int, Entity>(pow, monomialExpr);
            }

            Entity result = Number.Create(1);
            foreach(var child in monomialExpr.Children)
            {
                if (child == variable)
                {
                    pow++;
                }
                else
                {
                    if (monomialExpr.IsOperatorEntity(Constants.Operators.MUL, out OperatorEntity op))
                    {
                        result *= child;
                    }
                }
            }

            return new KeyValuePair<int, Entity>(pow, result);
        }

        private static ICollection<Entity> GetSumEntities(Entity entity)
        {
            var result = new List<Entity>();
            result.AddRange(entity.Children.Where(_ => entity.IsOperatorEntity(Constants.Operators.SUM, out OperatorEntity r)));
            result.Add(Number.Create(0));
            return result;
        }
    }
}
