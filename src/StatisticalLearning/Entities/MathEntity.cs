// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Entities
{
    public static class MathEntity
    {
        public static Entity Sqrt(Entity entity)
        {
            var result = new FuncEntity(Constants.Funcs.SQUAREROOT, entity);
            return result;
        }

        public static Entity Pow(Entity entity, Entity power)
        {
            var result = new FuncEntity(Constants.Funcs.POW, entity, power);
            return result;
        }

        public static Entity Acos(Entity entity)
        {
            var result = new FuncEntity(Constants.Funcs.ACOS, entity);
            return result;
        }

        public static Entity Cos(Entity entity)
        {
            var result = new FuncEntity(Constants.Funcs.COS, entity);
            return result;
        }
    }
}
