// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/Sync.Core/CommonExtension.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 28/06/2023
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sync.Core
{
	public static class CommonEnhancer
	{
		public static void Null<T>([NotNull] T? value, [CallerArgumentExpressionAttribute(parameterName:"value")] string? paramName = null)
		{
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        public static List<string> GetTablePrimaryKeys(IDbConnection connection, string tableName)
        {
            var primaryKeys = new List<string>();
            var query = $@"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                           WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME),
                           'IsPrimaryKey') = 1 AND TABLE_NAME = '{tableName}'";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader.GetString(0);
                        primaryKeys.Add(columnName);
                    }
                }
                
            }
            return primaryKeys;
        }
    }
}

