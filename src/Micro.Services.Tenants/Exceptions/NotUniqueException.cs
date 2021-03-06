﻿using System;

namespace Micro.Services.Tenants.Exceptions
{
    public class NotUniqueException : ApplicationException
    {
        public NotUniqueException(string entity, string property, string value)
            : base($"A {entity} with {property} equal to {value} already exists")
        {

        }
    }
}
