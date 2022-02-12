using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;

namespace Assets.Scripts.DB
{
    public static class SqlHelper
    {
        private static Dictionary<SqlDbType, Type> typeMap;

        static SqlHelper()
        {
            typeMap = new Dictionary<SqlDbType, Type>
{
    { SqlDbType.BigInt, typeof(long) },
    { SqlDbType.Binary, typeof(byte[]) },
    { SqlDbType.Bit, typeof(bool) },
    { SqlDbType.Char, typeof(string) },
    { SqlDbType.Date, typeof(DateTime) },
    { SqlDbType.DateTime, typeof(DateTime) },
    { SqlDbType.DateTime2, typeof(DateTime) }, // SQL2008+
    { SqlDbType.DateTimeOffset, typeof(DateTimeOffset) }, // SQL2008+
    { SqlDbType.Decimal, typeof(decimal) },
    { SqlDbType.Float, typeof(double) },
    { SqlDbType.Image, typeof(byte[]) },
    { SqlDbType.Int, typeof(int) },
    { SqlDbType.Money, typeof(decimal) },
    { SqlDbType.NChar, typeof(string) },
    { SqlDbType.NVarChar, typeof(string) },
    { SqlDbType.Real, typeof(float) },
    { SqlDbType.SmallDateTime, typeof(DateTime) },
    { SqlDbType.SmallInt, typeof(short) },
    { SqlDbType.SmallMoney, typeof(decimal) },
    { SqlDbType.Time, typeof(TimeSpan) }, // SQL2008+
    { SqlDbType.TinyInt, typeof(byte) },
    { SqlDbType.UniqueIdentifier, typeof(Guid) },
    { SqlDbType.VarBinary, typeof(byte[]) },
    { SqlDbType.VarChar, typeof(string) },
    { SqlDbType.Xml, typeof(SqlXml) }
    // omitted special types: timestamp
    // omitted deprecated types: ntext, text
    // not supported by enum: numeric, FILESTREAM, rowversion, sql_variant
};
        }

        public static Type getNetType(SqlDbType type)
        {
            if (typeMap.ContainsKey(type))
            {
                return typeMap[type];
            }
            throw new ArgumentException($"{type.ToString()} is not contained in the type database!");
        }

        public static Type getNetType(string type)
        {
            // TO DO : POPULATE
            switch (type)
            {
                case "tinyint":
                    return getNetType(SqlDbType.TinyInt);
                case "varchar":
                    return getNetType(SqlDbType.VarChar);
                case "real":
                    return getNetType(SqlDbType.Real);
                case "int":
                    return getNetType(SqlDbType.Int);
                default:
                    throw new ArgumentException($"{type} is not currently supported! Submit a ticket please!");
            }
        }

    }

}
