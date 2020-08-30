// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System;
using System.Text;

namespace StatisticalLearning.Math.Entities
{
    public class VariableEntity : Entity
    {
        private NumberEntity _number;
        public static implicit operator VariableEntity(string name) => new VariableEntity(name);

        public VariableEntity(string name, double nbTimes = 1, int pow = 1)
        {
            Name = name;
            NbTimes = nbTimes;
            Pow = pow;
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public Entity Mul(NumberEntity number)
        {
            var nbTimes = NbTimes * number.Number.Value;
            if (nbTimes == 0) 
            {
                return Number.Create(0);
            }

            return new VariableEntity(Name, nbTimes, Pow);
        }

        public Entity Mul(VariableEntity variable)
        {
            if (Name != variable.Name)
            {
                return this * variable;
            }

            var nbTimes = NbTimes * variable.NbTimes;
            return new VariableEntity(Name, nbTimes, Pow + variable.Pow);
        }

        public Entity Sum(VariableEntity variable)
        {
            if (Name != variable.Name || Pow != variable.Pow)
            {
                return this + variable;
            }

            var nbTimes = NbTimes + variable.NbTimes;
            if (nbTimes == 0)
            {
                return Number.Create(0);
            }

            return new VariableEntity(Name, nbTimes, variable.Pow);
        }

        public Entity Sub(VariableEntity variable)
        {
            if (Name != variable.Name || Pow != variable.Pow)
            {
                return this - variable;
            }

            var nbTimes = NbTimes - variable.NbTimes;
            if (nbTimes == 0)
            {
                return Number.Create(0);
            }

            return new VariableEntity(Name, nbTimes, variable.Pow);
        }

        public Entity Div(VariableEntity variable)
        {
            if (Name != variable.Name)
            {
                return this / variable;
            }

            throw new NotImplementedException();
        }

        public void AssignNumber(NumberEntity number)
        {
            _number = number;
        }

        public string Name { get; private set; }
        public double NbTimes { get; private set; }
        public int Pow { get; set; }

        public override int CompareTo(object obj)
        {
            return -1;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + NbTimes.GetHashCode() + Pow.GetHashCode();
        }

        public override Entity Derive()
        {
            return new NumberEntity(1);
        }

        public override Entity Eval()
        {
            if (_number != null)
            {
                return (Number.Create(NbTimes) * MathEntity.Pow(_number, Number.Create(Pow))).Eval();
            }

            return this;
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            if (NbTimes != 1)
            {
                strBuilder.Append(NbTimes);
            }

            strBuilder.Append(Name);
            if (Pow > 1)
            {
                strBuilder.Append($"^{Pow}");
            }

            return strBuilder.ToString();
        }
    }
}
