namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Enums
{
    public enum Operator
    {
        NONE,
        equal,
        not_equal,
        @in,
        not_in,
        less,
        less_or_equal,
        greater,
        greater_or_equal,
        begins_with,
        not_begins_with,
        contains,
        not_contains,
        ends_with,
        not_ends_with
    }
}
