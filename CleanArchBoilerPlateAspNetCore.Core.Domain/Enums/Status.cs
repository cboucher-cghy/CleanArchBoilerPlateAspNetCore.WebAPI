using System;
using System.ComponentModel;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Enums
{
    public enum FeatureStatus
    {
        [Description("In Work")]
        In_Work,
        [Description("Released")]
        Released,
        [Description("Archived")]
        Archived
    }

    public enum ContextSatus
    {
        [Description("In Work")]
        In_Work,
        [Description("In Approval")]
        In_Approval,
        [Description("Released")]
        Released,
        [Description("Archived")]
        Archived
    }

    /// <summary>
    /// Extension class for Enum to allow custom description.
    /// </summary>
    public static class Extensions
    {
        public static string Description(this Enum @enum)
        {
            var description = string.Empty;
            var fields = @enum.GetType().GetFields();
            foreach (var field in fields)
            {
                var descriptionAttribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (descriptionAttribute != null &&
                    field.Name.Equals(@enum.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    description = descriptionAttribute.Description;
                    break;
                }
            }

            return description;
        }
    }
}
