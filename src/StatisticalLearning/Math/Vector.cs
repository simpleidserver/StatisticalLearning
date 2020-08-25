// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using StatisticalLearning.Math.Entities;
using System;

namespace StatisticalLearning.Math
{
    public class Vector
    {
        private Entity[] _values;
        private VectorDirections _direction;
        public static implicit operator Vector(Entity[] values)
        {
            return new Vector(values);
        }
        public static implicit operator Vector(double[] values)
        {
            var arr = new Entity[values.Length];
            for (int row = 0; row < values.Length; row++)
            {
                arr[row] = values[row];
            }

            return new Vector(arr);
        }

        public Vector(int length)
        {
            Init(length);
        }

        public Vector(Entity[] values, VectorDirections direction = VectorDirections.HORIZONTAL)
        {
            _values = values;
            _direction = direction;
        }

        public int Length => _values.Length;
        public VectorDirections Direction => _direction;

        public Entity[] Values => _values;

        public Entity this[int key]
        {
            get => _values[key];
            set => _values[key] = value;
        }

        public double[] GetNumbers()
        {
            var result = new double[Length];
            for(int i = 0; i < _values.Length; i++)
            {
                result[i] = _values[i].GetNumber();
            }

            return result;
        }

        public Vector Abs()
        {
            var result = new Entity[Length];
            for (int i = 0; i < Length; i++)
            {
                result[i] = MathEntity.Abs(this[i]).Eval();
            }

            return new Vector(result);
        }

        public Vector Substract(Vector vector)
        {
            var result = new Entity[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                result[i] = (this[i] - vector[i]).Eval();
            }

            return new Vector(result);
        }

        public Vector Sum(Vector vector)
        {
            var result = new Entity[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                result[i] = (this[i] + vector[i]).Eval();
            }

            return new Vector(result);
        }

        public Vector Multiply(Vector vector)
        {
            var result = new Entity[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                result[i] = (this[i] * vector[i]).Eval();
            }

            return new Vector(result);
        }

        public Vector Transform(Func<Entity, Entity> callback)
        {
            var result = new Entity[Length];
            for(int i = 0; i < Length; i++)
            {
                result[i] = callback(_values[i]).Eval();
            }

            return new Vector(result);
        }

        public Entity Max()
        {
            Entity result = 0.0;
            for (int i = 0; i < Length; i++)
            {
                if (this[i] >= result)
                {
                    result = this[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Σ xi/n
        /// </summary>
        /// <returns></returns>
        public double Avg()
        {
            Entity totalSum = 0;
            foreach (var value in _values)
            {
                totalSum += value;
            }

            return (totalSum / _values.Length).Eval().GetNumber();
        }

        /// <summary>
        /// Σ(xi2)-(Σ xi)2/n
        /// </summary>
        /// <returns></returns>
        public double SumOfSquares()
        {
            var avg = Avg();
            return SumOfSquares(avg);
        }

        /// <summary>
        /// Σ(xi2)-(Σ xi)2/n
        /// </summary>
        /// <param name="avg"></param>
        /// <returns></returns>
        public double SumOfSquares(Entity avg)
        {
            Entity result = 0;
            foreach (var value in _values)
            {
                result += MathEntity.Pow((value - avg), 2);
            }

            return result.Eval().GetNumber();
        }

        /// <summary>
        /// Standard deviation.
        /// </summary>
        /// <param name="unbiased"></param>
        /// <returns></returns>
        public double StandardDeviation(bool unbiased = true)
        {
            var mean = Avg();
            Entity sum = 0;
            foreach (var input in _values)
            {
                sum += MathEntity.Pow(input - mean, 2);
            }

            if (unbiased)
            {
                return MathEntity.Sqrt(sum / (_values.Length - 1)).Eval().GetNumber();
            }

            return MathEntity.Sqrt(sum / (_values.Length)).Eval().GetNumber();
        }

        public Entity Sum(Func<Entity, Entity> callback)
        {
            Entity result = 0.0;
            for(int i = 0; i < _values.Length; i++)
            {
                result += callback(_values[i]);
            }

            return result;
        }

        public bool All(Func<Entity, bool> callback)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                if(!callback(_values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public Vector SetHorizontal()
        {
            _direction = VectorDirections.HORIZONTAL;
            return this;
        }

        public Vector SetVertical()
        {
            _direction = VectorDirections.VERTICAL;
            return this;
        }

        public Vector SwitchDirection()
        {
            if (Direction == VectorDirections.HORIZONTAL)
            {
                _direction = VectorDirections.VERTICAL;
            }
            else
            {
                _direction = VectorDirections.HORIZONTAL;
            }

            return this;
        }

        private void Init(int length)
        {
            _values = new Entity[length];
            for(int i = 0; i < length; i++)
            {
                _values[i] = 0;
            }
        }
    }
}
