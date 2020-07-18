﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Numeric;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalLearning.Entities
{
    public abstract class Entity
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

        public Entity Evaluate(VariableEntity v)
        {
            var tmp = Eval();
            Entity result = null;
            var children = GetSumOrSubEntities(tmp);
            var variables = children.Where(_ =>
            {
                var tmpVar = _ as VariableEntity;
                if (tmpVar == null)
                {
                    return false;
                }

                return v.Name == v.Name;
            }).Select(_ => _ as VariableEntity);
            var pows = variables.Select(_ => _.Pow).Distinct().OrderByDescending(_ => _);
            foreach(var pow in pows)
            {
                var powVariables = variables.Where(_ => _.Pow == pow);
                Entity sumResult = null;
                foreach(var powVariable in powVariables)
                {
                    if (sumResult == null)
                    {
                        sumResult = powVariable;
                    }
                    else
                    {
                        sumResult = (sumResult + powVariable).Eval();
                    }
                }

                if (result == null)
                {
                    result = sumResult;
                }
                else
                {
                    result += sumResult;
                }
            }

            var other = children.Where(_ =>
            {
                var tmpVar = _ as VariableEntity;
                if (tmpVar == null)
                {
                    return true;
                }

                return v.Name != v.Name;
            });
            foreach(var variable in other)
            {
                if (result == null)
                {
                    result = variable;
                }
                else
                {
                    result = result + variable;
                }
            }

            return result;
        }

        public abstract Entity Derive();
        public abstract Entity Eval();

        public IEnumerable<Entity> Solve(VariableEntity variable)
        {
            var dic = new Dictionary<int, Entity>();
            var children = GetSumOrSubEntities(this);
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

            if (dic.Count() == 1)
            {
                dic.Add(0, Number.Create(0));
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

                return new[] { value };
            }
            else if (orders.Count() == 3 && orders.ElementAt(2).Key == 2)
            {
                var a = orders.ElementAt(2).Value;
                var b = orders.ElementAt(1).Value;
                var c = orders.ElementAt(0).Value;
                var discriminant = b * b - Number.Create(4) * a * c;
                var number = discriminant.Eval() as NumberEntity;
                if (number != null)
                {
                    if (number.Number.Value == 0)
                    {
                        return new Entity[] { (Number.Create(-1) * b) / Number.Create(2) * a };
                    }

                    var x1 = (Number.Create(-1) * b - MathEntity.Sqrt(number)) / Number.Create(2) * a;
                    var x2 = (Number.Create(-1) * b + MathEntity.Sqrt(number)) / Number.Create(2) * a;
                    return new Entity[] { x1.Eval(), x2.Eval() };
                }
            }

            return null;
        }

        public bool IsOperatorEntity(out OperatorEntity result)
        {
            result = null;
            var r = this as OperatorEntity;
            if (r != null)
            {
                result = r;
                return true;
            }

            return false;
        }

        public bool IsOperatorEntity(IEnumerable<string> types, out OperatorEntity result)
        {
            result = null;
            var r = this as OperatorEntity;
            if (r != null && types.Contains(r.Name))
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

        public bool IsVariableEntity(out VariableEntity result)
        {
            result = this as VariableEntity;
            return result != null;
        }

        private static KeyValuePair<int, Entity> ParseMonomial(Entity monomialExpr, VariableEntity variable)
        {
            Func<Entity, VariableEntity> extractVariableEntity = (ent) =>
            {
                if (ent.IsVariableEntity(out VariableEntity varEnt) && varEnt.Name == variable.Name)
                {
                    return varEnt;
                }

                return null;
            };
            if (!monomialExpr.Children.Any())
            {
                var varEnt = extractVariableEntity(monomialExpr);
                if (varEnt != null)
                {
                    return new KeyValuePair<int, Entity>(varEnt.Pow, Number.Create(varEnt.NbTimes)) ;
                }

                return new KeyValuePair<int, Entity>(0, monomialExpr);
            }

            int pow = 0;
            Entity result = null;
            foreach(var child in monomialExpr.Children.SelectMany(_ => GetMulOrDivEntities(_)))
            {
                var varEnt = extractVariableEntity(child);
                if (varEnt != null)
                {
                    pow += varEnt.Pow;
                    if (result == null)
                    {
                        result = Number.Create(varEnt.NbTimes);
                    }
                    else
                    {
                        result *= Number.Create(varEnt.NbTimes);
                    }
                }
                else
                {
                    if (monomialExpr.IsOperatorEntity(new[] { Constants.Operators.MUL }, out OperatorEntity op))
                    {
                        if (result == null)
                        {
                            result = child;
                        }
                        else
                        {
                            result *= child;
                        }
                    }
                }
            }

            return new KeyValuePair<int, Entity>(pow, result);
        }

        private static ICollection<Entity> GetSumOrSubEntities(Entity entity)
        {
            var result = new List<Entity>();
            if (entity.IsOperatorEntity(new[] { Constants.Operators.SUM, Constants.Operators.SUB }, out OperatorEntity e))
            {
                var a = entity.Children.ElementAt(0);
                var b = entity.Children.ElementAt(1);
                result.AddRange(GetSumOrSubEntities(a));
                var children = GetSumOrSubEntities(b).ToArray();
                if(e.Name == Constants.Operators.SUB)
                {
                    for (int i = 0; i < children.Count(); i++)
                    {
                        children[i] = Number.Create(-1) * children[i];
                    }
                }

                result.AddRange(children);
            }
            else
            {
                result.Add(entity);
            }

            return result;
        }

        private static ICollection<Entity> GetMulOrDivEntities(Entity entity)
        {
            var result = new List<Entity>();
            if (entity.IsOperatorEntity(new[] { Constants.Operators.MUL, Constants.Operators.DIV }, out OperatorEntity e))
            {
                var a = entity.Children.ElementAt(0);
                var b = entity.Children.ElementAt(1);
                result.AddRange(GetMulOrDivEntities(a));
                result.AddRange(GetMulOrDivEntities(b));
            }
            else
            {
                result.Add(entity);
            }

            // result.Add(Number.Create(0));
            return result;
        }
    }
}
