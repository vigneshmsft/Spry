using System;
using Spry.Select;

namespace Spry.Table
{
    public class LeftOuterJoin<TDto> : Join<TDto>
    {
        public LeftOuterJoin(SprySelect<TDto> spry, SprySelectTable<TDto> tableOne, SprySelectTable<TDto> tableTwo)
            : base(spry, tableOne, tableTwo)
        {
        }

        internal override string BuildImpl()
        {
            return "LEFT OUTER JOIN " + TableTwo.BuildImpl() + OnCondition + Environment.NewLine;
        }
    }
}
