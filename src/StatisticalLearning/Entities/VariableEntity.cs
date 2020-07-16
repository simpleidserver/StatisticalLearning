// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Entities
{
    public class VariableEntity : Entity
    {
        public static implicit operator VariableEntity(string name) => new VariableEntity(name);

        public VariableEntity(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override Entity Derive()
        {
            return new NumberEntity(1);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
