using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CleanArchBoilerPlateAspNetCore.WebAPI.Extensions
{
    /// <summary>
    /// Add specific details in the section "Extensions" of the instance.
    /// </summary>
    public static class ProblemDetailsExtensions
    {
        // Source: https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/cc645611(v=sql.105)
        private const int SqlServerViolationOfUniqueIndex = 2601;
        private const int SqlServerViolationOfNull = 515;
        private const int SqlServerViolationOfUniqueConstraint = 2627;

        /// <summary>
        /// Add details regarding any Entity framework Core exception.
        /// </summary>
        /// <param name="problem">Instance of the ProblemDetails.</param>
        /// <param name="ex">Exception in which search for more details.</param>
        public static void AddDatabaseExceptionDetailsAsExtensionDetails(this ProblemDetails problem, Exception ex)
        {
            if (ex == null)
            {
                // Nothing we can do here...
                return;
            }

            //This is a DbUpdateException on a SQL database
            if (ex is DbUpdateException)
            {
                var dbUpdateEx = ex as DbUpdateException;
                problem.Extensions["DbUpdateException"] = dbUpdateEx.Message;

                // Is there an inner SqlException ? Occurs when errors on i.e. "insert/update"
                Exception innerEx = dbUpdateEx.InnerException;

                if (dbUpdateEx.InnerException is SqlException)
                {
                    var sqlEx = innerEx as SqlException;
                    problem.Extensions["DbUpdateException.SqlException"] = sqlEx.Message;

                    if (sqlEx.Number == SqlServerViolationOfUniqueIndex ||
                            sqlEx.Number == SqlServerViolationOfUniqueConstraint)
                    {
                        // We have an error we can process
                        SqlUniqueError sqlStruct = SqlUniqueErrorFormatter.UniqueErrorFormatter(sqlEx, dbUpdateEx.Entries);

                        problem.Extensions["DbUpdateException.SqlException.DuplicateProperty"] = sqlStruct.DuplicateProperty;
                        problem.Extensions["DbUpdateException.SqlException.DuplicateValue"] = sqlStruct.DuplicateValue;
                        problem.Extensions["DbUpdateException.SqlException.EntityDisplayName"] = sqlStruct.EntityDisplayName;
                    }
                    else if (sqlEx.Number == SqlServerViolationOfNull)
                    {
                        StringBuilder sb = new StringBuilder();
                        // We have an error we can process
                        problem.Extensions["DbUpdateException.SqlException.NullEntity"] = string.Join(";", dbUpdateEx.Entries);

                        // TODO: Validate if we want to push the real name of the tables to the client... ?
                        //SqlNullError sqlStruct = SqlUniqueErrorFormatter.NullErrorFormatter(sqlEx, dbUpdateEx.Entries);
                        //problem.Extensions["DbUpdateException.SqlException.NullValue"] = sqlStruct.Column;
                        //problem.Extensions["DbUpdateException.SqlException.NullTable"] = sqlStruct.Table;
                    }

                    //else check for other SQL errors
                }
            }
        }

        /// <summary>
        /// Add the current ActivityId (or, if null, the HttpContext TraceIdentifier) as details.
        /// </summary>
        /// <param name="problem">Instance of the ProblemDetails.</param>
        /// <param name="context">Current HttpContext.</param>
        public static void AddTraceIdAsExtensionDetails(this ProblemDetails problem, HttpContext context)
        {
            // This is often very handy information for tracing the specific request
            var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
            if (traceId != null)
            {
                problem.Extensions["traceId"] = traceId;
            }
        }
    }

    internal class SqlUniqueError
    {
        public string DuplicateProperty { get; set; }
        public string EntityDisplayName { get; set; }
        public string DuplicateValue { get; set; }
    }

    internal class SqlNullError
    {
        public string Column { get; set; }
        public string Table { get; set; }
    }

    static class SqlUniqueErrorFormatter
    {
        private static readonly Regex _uniqueConstraintRegex = new Regex("'UniqueError_([a-zA-Z0-9]*)_([a-zA-Z0-9]*)'", RegexOptions.Compiled);

        private static readonly Regex _nullConstraintRegex = new Regex("Cannot insert the value NULL into column \'(.*)\', table \'(.*)\'", RegexOptions.Compiled);

        /// <summary>
        /// Parse the SqlException message to find the fields in error in the insert/update exception.
        /// </summary>
        /// <param name="ex">SqlException which contains the message in which we need to search for the values.</param>
        /// <param name="entitiesNotSaved"></param>
        /// <returns></returns>
        /// <remarks>Credits to: https://www.thereformedprogrammer.net/entity-framework-core-validating-data-and-catching-sql-errors/</remarks>
        public static SqlUniqueError UniqueErrorFormatter(SqlException ex, IReadOnlyList<EntityEntry> entitiesNotSaved)
        {
            var message = ex.Errors[0].Message;
            var matches = _uniqueConstraintRegex.Matches(message);

            if (matches.Count == 0)
                return null;

            var entityDisplayName = entitiesNotSaved.Count == 1
                ? entitiesNotSaved.Single().Entity.GetType().Name
                : matches[0].Groups[1].Value;

            SqlUniqueError sqlStruct = new SqlUniqueError()
            {
                DuplicateProperty = matches[0].Groups[2].Value,
                EntityDisplayName = entityDisplayName,
            };

            // Find the duplicated value, if available.
            var openingBadValue = message.IndexOf("(");
            if (openingBadValue > 0)
            {
                sqlStruct.DuplicateValue = message.Substring(openingBadValue + 1,
                   message.Length - openingBadValue - 3);
            }

            return sqlStruct;
        }

        public static SqlNullError NullErrorFormatter(SqlException ex, IReadOnlyList<EntityEntry> entitiesNotSaved)
        {
            var message = ex.Errors[0].Message;
            var matches = _nullConstraintRegex.Matches(message);

            if (matches.Count == 0)
                return null;

            var entityDisplayName = entitiesNotSaved.Count == 1
                ? entitiesNotSaved.Single().Entity.GetType().Name
                : matches[0].Groups[1].Value;

            SqlNullError sqlStruct = new SqlNullError()
            {
                Column = matches[0].Groups[1].Value,
                Table = matches[0].Groups[2].Value,
            };

            return sqlStruct;
        }
    }
}
