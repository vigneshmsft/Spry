using System;
using System.Collections.Generic;
using System.Text;
using Spry.Table;

namespace Spry.Where
{
    public class Where<TDto, TProperty> : Buildable
    {
        private readonly SpryTable<TDto> _table;
        private readonly SpryParameters _parameters;
        private readonly string _columnName;
        private readonly string _columnParameterName;
        private readonly StringBuilder _whereBuilder;

        internal Where(SpryTable<TDto> table, SpryParameters parameters, string columnName)
        {
            _whereBuilder = new StringBuilder();
            _table = table;
            _parameters = parameters;
            _columnName = columnName;
            _columnParameterName = CleanColumnName(_columnName);
        }

        public SpryTable<TDto> EqualTo(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} = @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public SpryTable<TDto> In(IEnumerable<TProperty> value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} IN @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public SpryTable<TDto> GreaterThan(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} > @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public SpryTable<TDto> LessThan(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} < @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public SpryTable<TDto> LessThanOrEqualTo(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} <= @{1}", _columnName, _columnParameterName);
            return _table;
        }


        public SpryTable<TDto> GreaterThanOrEqualTo(TProperty value)
        {
            _parameters.Add(_columnParameterName, value);
            _whereBuilder.AppendFormat(@"{0} >= @{1}", _columnName, _columnParameterName);
            return _table;
        }

        public SpryTable<TDto> InBetween(TProperty valueOne, TProperty valueTwo)
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

        private string CleanColumnName(string columnName)
        {
            return columnName.Replace("@", "").Replace(".", "");
        }
    }
}