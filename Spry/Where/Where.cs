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
        private readonly StringBuilder _whereBuilder;

        internal Where(SpryTable<TDto> table, SpryParameters parameters, string columnName)
        {
            _whereBuilder = new StringBuilder();
            _table = table;
            _parameters = parameters;
            _columnName = columnName;
        }

        public SpryTable<TDto> EqualTo(TProperty value)
        {
            _parameters.Add(_columnName, value);
            _whereBuilder.AppendFormat(@"{0} = @{0}", _columnName);
            return _table;
        }

        public SpryTable<TDto> In(IEnumerable<TProperty> value)
        {
            _parameters.Add(_columnName, value);
            _whereBuilder.AppendFormat(@"{0} IN @{0}", _columnName);
            return _table;
        }

        public SpryTable<TDto> GreaterThan(TProperty value)
        {
            _parameters.Add(_columnName, value);
            _whereBuilder.AppendFormat(@"{0} > @{0}", _columnName);
            return _table;
        }

        public SpryTable<TDto> LessThan(TProperty value)
        {
            _parameters.Add(_columnName, value);
            _whereBuilder.AppendFormat(@"{0} < @{0}", _columnName);
            return _table;
        }

        public SpryTable<TDto> LessThanOrEqualTo(TProperty value)
        {
            _parameters.Add(_columnName, value);
            _whereBuilder.AppendFormat(@"{0} <= @{0}", _columnName);
            return _table;
        }


        public SpryTable<TDto> GreaterThanOrEqualTo(TProperty value)
        {
            _parameters.Add(_columnName, value);
            _whereBuilder.AppendFormat(@"{0} >= @{0}", _columnName);
            return _table;
        }

        public SpryTable<TDto> InBetween(TProperty valueOne, TProperty valueTwo)
        {
            _parameters.Add(_columnName, valueOne);
            _parameters.Add(_columnName + valueTwo, valueTwo);
            _whereBuilder.AppendFormat(@"{0} BETWEEN @{0} AND @{0}valueTwo", _columnName);
            return _table;
        }

        internal override string BuildImpl()
        {
            return _whereBuilder + Environment.NewLine;
        }
    }
}