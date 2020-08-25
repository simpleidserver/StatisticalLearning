// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System;
using System.Linq;

namespace StatisticalLearning.Math.Entities
{
    public class FuncEntity : Entity
    {
        public FuncEntity(string name, params Entity[] children)
        {
            Name = name;
            Children = children.ToList();
        }

        public string Name { get; private set; }

        public override int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public override Entity Derive()
        {
            switch(Name)
            {
                case Constants.Funcs.POW:
                    var secondChild = Children.Last().Eval();
                    if (secondChild.IsNumberEntity(out NumberEntity subResult))
                    {

                    }

                    break;
            }

            return null;
        }

        public override Entity Eval()
        {
            Entity res = null;
            var firstChild = Children.First().Eval();
            if (firstChild.IsNumberEntity(out NumberEntity result))
            {
                switch(Name)
                {
                    case Constants.Funcs.SQUAREROOT:
                        res = Number.Create(System.Math.Sqrt(result.Number.Value));
                        break;
                    case Constants.Funcs.POW:
                        var secondChild = Children.Last().Eval();
                        if (secondChild.IsNumberEntity(out NumberEntity subResult))
                        {
                            var value = result.Number.Value;
                            if (value == 0)
                            {
                                res = result.Number;
                            }
                            else
                            {
                                res = Number.Create(System.Math.Pow(value, subResult.Number.Value));
                            }
                        }

                        break;
                    case Constants.Funcs.ACOS:
                        res = Number.Create(System.Math.Acos(result.Number.Value));
                        break;
                    case Constants.Funcs.COS:
                        res = Number.Create(System.Math.Cos(result.Number.Value));
                        break;
                    case Constants.Funcs.ABS:
                        res = System.Math.Abs(result.Number.Value);
                        break;
                    case Constants.Funcs.EXP:
                        res = System.Math.Exp(result.Number.Value);
                        break;
                }
            }

            return res;
        }
        public override string ToString()
        {
            var firstChild = Children.First();
            var o = Constants.MappingFuncToSign[Name];
            return $"{o} {firstChild.ToString()}";
        }
    }
}
