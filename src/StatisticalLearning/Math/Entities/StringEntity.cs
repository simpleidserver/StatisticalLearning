// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace StatisticalLearning.Math.Entities
{
    public class StringEntity : Entity
    {
        public static bool operator ==(StringEntity a, StringEntity b)
        {
            if (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null)) return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;
            return a.Str == b.Str;
        }
        public static bool operator !=(StringEntity a, StringEntity b)
        {
            if (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null)) return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;

            return a.Str != b.Str;
        }

        public StringEntity(string str)
        {
            Str = str;
        }

        public string Str { get; set; }

        public bool Equals(StringEntity other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return this.GetHashCode().Equals(other);
        }

        public override int CompareTo(object obj)
        {
            var o = obj as StringEntity;
            if (o == null)
            {
                return -1;
            }

            return o.Str.CompareTo(Str);
        }

        public override int GetHashCode()
        {
            return Str.GetHashCode();
        }

        public override Entity Derive()
        {
            return null;
        }

        public override Entity Eval()
        {
            return this;
        }

        public override string ToString()
        {
            return Str;
        }
    }
}
