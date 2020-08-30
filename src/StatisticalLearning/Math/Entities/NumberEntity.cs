// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StatisticalLearning.Math.Entities
{
    public class NumberEntity : Entity
    {
        public static bool operator ==(NumberEntity a, NumberEntity b)
        {
            if (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null))
            {
                return true;
            }

            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Number.Value == b.Number.Value;
        }
        public static bool operator !=(NumberEntity a, NumberEntity b)
        {
            if (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null))
            {
                return false;
            }

            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
            {
                return true;
            }

            return a.Number.Value != b.Number.Value;
        }
        public static bool operator <=(NumberEntity a, NumberEntity b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a.Number.Value <= b.Number.Value;
        }
        public static bool operator <(NumberEntity a, NumberEntity b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a.Number.Value <= b.Number.Value;
        }
        public static bool operator >=(NumberEntity a, NumberEntity b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a.Number.Value >= b.Number.Value;
        }
        public static bool operator >(NumberEntity a, NumberEntity b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a.Number.Value > b.Number.Value;
        }

        public NumberEntity(Number number)
        {
            Number = number;
        }

        public Number Number { get; set; }

        public override int CompareTo(object obj)
        {
            var source = obj as NumberEntity;
            if (source == null)
            {
                return -1;
            }

            return Number.Value.CompareTo(source.Number.Value);
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }


        public override Entity Derive()
        {
            return new NumberEntity(new Number(0));
        }

        public override Entity Eval()
        {
            return Number;
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
