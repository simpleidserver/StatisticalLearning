// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
namespace StatisticalLearning.Api.Host.DTOs.Requests
{
    public class GetStudentLawRequest
    {
        public double TStat { get; set; } 
        public double DegreeOfFreedom { get; set; }
    }
}
