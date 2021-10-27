using System;

namespace NewsTrack.Domain.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(Guid? id) : 
            base(id == null ? "Data not found" : $"Object with id '{id}' not found")
        {
        }
    }
}
