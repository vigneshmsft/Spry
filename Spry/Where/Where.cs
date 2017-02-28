using System;
using System.Collections.Generic;
using System.Text;
using Spry.Table;

namespace Spry.Where
{
    public class Where<TDto, TProperty, TTable> : Buildable where TTable : SpryTable<TDto, TTable>
    {
        private readonly TTable _table;
        private readonly SpryParameters _parameters;
        private readonly string _columnName;
        private readonly string _columnParameterName;
        private readonly StringBuilder _whereBuilder;

        internal Where(TTable table, SpryParameters parameters, string columnName)
        {
            _whereBuilder = new StringBuilder();
            _table = table;
            _parameters = parameters;
            _columnName = columnName.Replace(";", "");
            _columnParameterName = CleanParameterName(_columnName);
        }

        public TTable EqualTo(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} = @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public TTable In(IEnumerable<TProperty> value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} IN @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public TTable GreaterThan(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} > @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public TTable LessThan(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} < @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public TTable LessThanOrEqualTo(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} <= @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public TTable GreaterThanOrEqualTo(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} >= @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public TTable InBetween(TProperty valueOne, TProperty valueTwo)
        {
            _parameters.Add(_columnParameterName, valueOne);
            _parameters.Add(_columnParameterName + "valueTwo", valueTwo);
            _whereBuilder.AppendFormat(@"{0} BETWEEN @{1} AND @{1}valueTwo", _columnName, _columnParameterName);
            return _table;
        }

        internal override string BuildImpl()
        {
            return _whereBuilder + Environment.NewLine;
        }

        private static string CleanParameterName(string columnName)
        {
            return "p" + columnName.Replace("@", "").Replace(".", "").Replace(";","");
        }
    }
}