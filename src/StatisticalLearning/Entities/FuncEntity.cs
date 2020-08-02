// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Numeric;
using System;

namespace StatisticalLearning.Entities
{
    public class FuncEntity : Entity
    {
        public FuncEntity(string name, Entity child, Entity subChild = null)
        {
            Name = name;
            Child = child;
            SubChild = subChild;
        }

        public string Name { get; private set; }
        public Entity Child { get; private set; }
        public Entity SubChild { get; private set; }

        public override int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public override Entity Derive()
        {
            throw new NotImplementedException();
        }

        public override Entity Eval()
        {
            Entity res = null;
            var evaluated = Child.Eval();
            if (evaluated.IsNumberEntity(out NumberEntity result))
            {
                switch(Name)
                {
                    case Constants.Funcs.SQUAREROOT:
                        res = Number.Create(System.Math.Sqrt(result.Number.Value));
                        break;
                    case Constants.Funcs.POW:
                        if (SubChild.IsNumberEntity(out NumberEntity subResult))
                        {
                            var value = result.Number.Value;
                            bool isNegative = value < 0;
                            if (isNegative)
                            {
                                value = -value;
                            }

                            res = Number.Create(System.Math.Pow(value, subResult.Number.Value));
                            if (isNegative)
                            {
                                res = Number.Create(-1) * res;
                            }
                        }

                        break;
                    case Constants.Funcs.ACOS:
                        res = Number.Create(System.Math.Acos(result.Number.Value));
                        break;
                    case Constants.Funcs.COS:
                        res = Number.Create(System.Math.Cos(result.Number.Value));
                        break;
                }
            }

            return res;
        }
        public override string ToString()
        {
            var o = Constants.MappingFuncToSign[Name];
            return $"{o} {Child.ToString()}";
        }
    }
}
