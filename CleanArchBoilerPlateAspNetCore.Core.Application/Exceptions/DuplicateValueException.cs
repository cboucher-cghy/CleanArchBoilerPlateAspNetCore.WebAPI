using System;
using System.Runtime.Serialization;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.Exceptions
{
    /// <summary>
    /// Exception used when there is already an object with the same value for a given property and it is not allowed.
    /// </summary>
    [Serializable()]
    public class DuplicateValueException : Exception
    {
        private string _entityName;
        private string _propertyName;
        private object _value;

        protected DuplicateValueException()
           : base()
        { }

        public DuplicateValueException(string entityName, string propertyName, object value) :
           base(string.Format("The property \"{0}\" on the object \"{1}\" with the value \"{2}\" already exist.", propertyName, entityName, value))
        {
            _entityName = entityName;
            _propertyName = propertyName;
            _value = value;
        }

        public DuplicateValueException(string entityName, string propertyName, object value, string message)
           : base(message)
        {
            _entityName = entityName;
            _propertyName = propertyName;
            _value = value;
        }

        public DuplicateValueException(string entityName, string propertyName, object value, string message, Exception innerException) :
           base(message, innerException)
        {
            _entityName = entityName;
            _propertyName = propertyName;
            _value = value;
        }

        protected DuplicateValueException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        { }

        public object Value { get { return _value; } }
        public object PropertyName { get { return _propertyName; } }
        public object EntityName { get { return _entityName; } }
    }
}
