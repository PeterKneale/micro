using System;

namespace Micro.Services.Tenants.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string entity, string property, string value) 
            : base($"A {entity} with {property} {value} could not be found")
        {

        }

        public NotFoundException(string entity, string property, int value)
           : this(entity,property,value.ToString())
        {

        }
    }
}
