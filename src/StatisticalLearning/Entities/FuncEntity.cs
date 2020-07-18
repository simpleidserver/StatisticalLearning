// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Numeric;
using System;

namespace StatisticalLearning.Entities
{
    public class FuncEntity : Entity
    {
        public FuncEntity(string name, Entity child)
        {
            Name = name;
            Child = child;
        }

        public string Name { get; private set; }
        public Entity Child { get; private set; }

        public override Entity Derive()
        {
            throw new NotImplementedException();
        }

        public override Entity Eval()
        {
            Entity res = null;
            if (Child.IsNumberEntity(out NumberEntity result))
            {
                switch(Name)
                {
                    case Constants.Funcs.SQUAREROOT:
                        res = Number.Create(System.Math.Sqrt(result.Number.Value));
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
