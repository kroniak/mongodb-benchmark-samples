using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MongodbTransactions.Utils
{
    public static class GenericListDataReaderExtensions
    {
        public static GenericListDataReader<T> GetDataReader<T>(this IEnumerable<T> list) => new GenericListDataReader<T>(list);
    }

    public class GenericListDataReader<T> : IDataReader
    {
        private readonly IEnumerator<T> _list;
        private readonly List<PropertyInfo> _properties = new List<PropertyInfo>();
        private readonly Dictionary<string, int> _nameLookup = new Dictionary<string, int>();

        public GenericListDataReader(IEnumerable<T> list)
        {
            _list = list.GetEnumerator();

            _properties.AddRange(
                typeof(T)
                    .GetProperties(
                        BindingFlags.GetProperty |
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.DeclaredOnly
                    ));

            for (var i = 0; i < _properties.Count; i++)
            {
                _nameLookup[_properties[i].Name] = i;
            }
        }

        #region IDataReader Members

        public void Close()
        {
            _list.Dispose();
        }

        public int Depth => throw new NotImplementedException();

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed => throw new NotImplementedException();

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public bool Read()
        {
            return _list.MoveNext();
        }

        public int RecordsAffected => throw new NotImplementedException();

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion

        #region IDataRecord Members

        public int FieldCount => _properties.Count;

        public bool GetBoolean(int i)
        {
            return (bool) GetValue(i);
        }

        public byte GetByte(int i)
        {
            return (byte) GetValue(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return (char) GetValue(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime) GetValue(i);
        }

        public decimal GetDecimal(int i)
        {
            return (decimal) GetValue(i);
        }

        public double GetDouble(int i)
        {
            return (double) GetValue(i);
        }

        public Type GetFieldType(int i)
        {
            return _properties[i].PropertyType;
        }

        public float GetFloat(int i)
        {
            return (float) GetValue(i);
        }

        public Guid GetGuid(int i)
        {
            return (Guid) GetValue(i);
        }

        public short GetInt16(int i)
        {
            return (short) GetValue(i);
        }

        public int GetInt32(int i)
        {
            return (int) GetValue(i);
        }

        public long GetInt64(int i)
        {
            return (long) GetValue(i);
        }

        public string GetName(int i)
        {
            return _properties[i].Name;
        }

        public int GetOrdinal(string name)
        {
            if (_nameLookup.ContainsKey(name))
            {
                return _nameLookup[name];
            }
            else
            {
                return -1;
            }
        }

        public string GetString(int i)
        {
            return (string) GetValue(i);
        }

        public object GetValue(int i)
        {
            return _properties[i].GetValue(_list.Current, null);
        }

        public int GetValues(object[] values)
        {
            var getValues = Math.Max(FieldCount, values.Length);

            for (var i = 0; i < getValues; i++)
            {
                values[i] = GetValue(i);
            }

            return getValues;
        }

        public bool IsDBNull(int i)
        {
            return GetValue(i) == null;
        }

        public object this[string name] => GetValue(GetOrdinal(name));

        public object this[int i] => GetValue(i);

        #endregion
    }
}