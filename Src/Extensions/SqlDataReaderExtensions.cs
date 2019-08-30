using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System.Data.SqlClient.CommandManager.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string name)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public static bool HasColumnAndValue(this SqlDataReader reader, string name)
        {
            return reader.HasColumn(name) && !reader.IsDBNull(reader.GetOrdinal(name));
        }
        public static bool GetBoolean(this SqlDataReader reader, string name) => reader.GetBoolean(reader.GetOrdinal(name));
        public static byte GetByte(this SqlDataReader reader, string name) => reader.GetByte(reader.GetOrdinal(name));
        public static char GetChar(this SqlDataReader reader, string name) => reader.GetChar(reader.GetOrdinal(name));
        public static DateTime GetDateTime(this SqlDataReader reader, string name) => reader.GetDateTime(reader.GetOrdinal(name));
        public static DateTimeOffset GetDateTimeOffset(this SqlDataReader reader, string name) => reader.GetDateTimeOffset(reader.GetOrdinal(name));
        public static decimal GetDecimal(this SqlDataReader reader, string name) => reader.GetDecimal(reader.GetOrdinal(name));
        public static double GetDouble(this SqlDataReader reader, string name) => reader.GetDouble(reader.GetOrdinal(name));
        public static float GetFloat(this SqlDataReader reader, string name) => reader.GetFloat(reader.GetOrdinal(name));
        public static Guid GetGuid(this SqlDataReader reader, string name) => reader.GetGuid(reader.GetOrdinal(name));
        public static Int16 GetInt16(this SqlDataReader reader, string name) => reader.GetInt16(reader.GetOrdinal(name));
        public static Int32 GetInt32(this SqlDataReader reader, string name) => reader.GetInt32(reader.GetOrdinal(name));
        public static Int64 GetInt64(this SqlDataReader reader, string name) => reader.GetInt64(reader.GetOrdinal(name));
        public static Stream GetStream(this SqlDataReader reader, string name) => reader.GetStream(reader.GetOrdinal(name));
        public static string GetString(this SqlDataReader reader, string name) => reader.GetString(reader.GetOrdinal(name));
        public static TimeSpan GetTimeSpan(this SqlDataReader reader, string name) => reader.GetTimeSpan(reader.GetOrdinal(name));
        public static object GetValue(this SqlDataReader reader, string name) => reader.GetValue(reader.GetOrdinal(name));

        private static T GetNullable<T>(SqlDataReader reader, string name, Func<string, T> action)
        {
            return reader.HasColumnAndValue(name)
                ? action(name)
                : default(T);
        }
        public static bool? GetBooleanNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetBoolean);
        public static byte? GetByteNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetByte);
        public static char? GetCharNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetChar);
        public static DateTime? GetDateTimeNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetDateTime);
        public static DateTimeOffset? GetDateTimeOffsetNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetDateTimeOffset);
        public static decimal? GetDecimalNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetDecimal);
        public static double? GetDoubleNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetDouble);
        public static float? GetFloatNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetFloat);
        public static Guid? GetGuidNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetGuid);
        public static Int16? GetInt16Nullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetInt16);
        public static Int32? GetInt32Nullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetInt32);
        public static Int64? GetInt64Nullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetInt64);
        public static Stream GetStreamNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetStream);
        public static string GetStringNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetString);
        public static TimeSpan? GetTimeSpanNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetTimeSpan);
        public static object GetValueNullable(this SqlDataReader reader, string name) => GetNullable(reader, name, reader.GetValue);
    }
}