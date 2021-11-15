using System;
using System.Runtime.Serialization;

namespace CleanArchBoilerPlateAspNetCore.Core.Application.Exceptions
{
    /// <summary>
    /// Exception used when the value
    /// </summary>
    [Serializable()]
    public class InvalidValueException : Exception
    {
        private string _entityName;
        private string _propertyName;
        private object _value;

        protected InvalidValueException()
           : base()
        { }

        public InvalidValueException(string entityName, string propertyName, object value) :
           base(string.Format("The value \"{0}\" for the property \"{1}\" on the object \"{2}\" is not valid.", value, propertyName, entityName))
        {
            _entityName = entityName;
            _propertyName = propertyName;
            _value = value;
        }

        public InvalidValueException(string entityName, string propertyName, object value, string message)
           : base(message)
        {
            _entityName = entityName;
            _propertyName = propertyName;
            _value = value;
        }

        public InvalidValueException(string entityName, string propertyName, object value, string message, Exception innerException) :
           base(message, innerException)
        {
            _entityName = entityName;
            _propertyName = propertyName;
            _value = value;
        }

        protected InvalidValueException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        { }

        public object Value { get { return _value; } }
        public object PropertyName { get { return _propertyName; } }
        public object EntityName { get { return _entityName; } }
    }
}
