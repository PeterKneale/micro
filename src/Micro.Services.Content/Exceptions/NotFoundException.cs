using System;

namespace Micro.Services.Content.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string entity, string property, string value) 
            : base($"A {entity} with {property} equal to {value} could not be found")
        {

        }

        public NotFoundException(string entity, string property, int value)
           : this(entity,property,value.ToString())
        {

        }
    }
}
