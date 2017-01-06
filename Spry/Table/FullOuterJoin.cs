using System;
using Spry.Select;

namespace Spry.Table
{
    public class FullOuterJoin<TDto> : Join<TDto>
    {
        public FullOuterJoin(SprySelect<TDto> spry, SprySelectTable<TDto> tableOne, SprySelectTable<TDto> tableTwo)
            : base(spry, tableOne, tableTwo)
        {
        }

        internal override string BuildImpl()
        {
            return "FULL OUTER JOIN " + TableTwo.BuildImpl() + OnCondition + Environment.NewLine;
        }
    }
}