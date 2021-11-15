using System;
using System.Runtime.Serialization;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.Exceptions
{
    public class NotDeletableException : Exception
    {
        private string _entityName;
        private object _id;

        protected NotDeletableException()
           : base()
        { }

        public NotDeletableException(string entityName, object id) :
           base(string.Format("The \"{0}\" with the Id \"{1}\" is not deletable.", entityName, id))
        {
            _entityName = entityName;
            _id = id;
        }

        public NotDeletableException(string entityName, object id, string message)
           : base(message)
        {
            _entityName = entityName;
            _id = id;
        }

        public NotDeletableException(string entityName, object id, string message, Exception innerException) :
           base(message, innerException)
        {
            _entityName = entityName;
            _id = id;
        }

        protected NotDeletableException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        { }

        public object Id { get { return _id; } }
        public object EntityName { get { return _entityName; } }
    }
}
