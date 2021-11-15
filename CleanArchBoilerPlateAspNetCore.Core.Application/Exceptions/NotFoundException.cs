using System;
using System.Runtime.Serialization;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.Exceptions
{
    /// <summary>
    /// Exception used when the requested entity is not found in the DB.
    /// </summary>
    [Serializable()]
    public class NotFoundException : Exception
    {
        private string _entityName;
        private object _id;

        protected NotFoundException()
           : base()
        { }

        public NotFoundException(string entityName, object id) :
           base(string.Format("The \"{0}\" with the Id \"{1}\" doesn't exist.", entityName, id))
        {
            _entityName = entityName;
            _id = id;
        }

        public NotFoundException(string entityName, object id, string message)
           : base(message)
        {
            _entityName = entityName;
            _id = id;
        }

        public NotFoundException(string entityName, object id, string message, Exception innerException) :
           base(message, innerException)
        {
            _entityName = entityName;
            _id = id;
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        { }

        public object Id { get { return _id; } }
        public object EntityName { get { return _entityName; } }
    }
}
