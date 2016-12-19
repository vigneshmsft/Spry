using System;
using Spry.Select;

namespace Spry.Table
{
    public class InnerJoin<TDto>
    {
        private readonly SprySelect<TDto> _spry;
        private readonly SprySelectTable<TDto> _tableOne;
        private readonly SprySelectTable<TDto> _tableTwo;
        private string _onCondition;

        public InnerJoin(SprySelect<TDto> spry, SprySelectTable<TDto> tableOne, SprySelectTable<TDto> tableTwo)
        {
            _spry = spry;
            _tableOne = tableOne;
            _tableTwo = tableTwo;
        }

        public SprySelectTable<TDto> On(string colOne, string colTwo)
        {
            _onCondition = " ON " + colOne + " = " + colTwo;
            return _tableOne;
        }

        internal string BuildImpl()
        {
            return "INNER JOIN " + _tableTwo.BuildImpl() + _onCondition + Environment.NewLine;
        }

        public string Build()
        {
            return _spry.Build();
        }
    }

}
