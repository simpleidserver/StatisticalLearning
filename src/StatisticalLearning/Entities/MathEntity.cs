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
    }
}
