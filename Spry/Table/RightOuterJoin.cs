using System;
using Spry.Select;

namespace Spry.Table
{
    public class RightOuterJoin<TDto> : Join<TDto>
    {
        public RightOuterJoin(SprySelect<TDto> spry, SprySelectTable<TDto> tableOne, SprySelectTable<TDto> tableTwo)
            : base(spry, tableOne, tableTwo)
        {
        }

        internal override string BuildImpl()
        {
            return "RIGHT OUTER JOIN " + TableTwo.BuildImpl() + OnCondition + Environment.NewLine;
        }
    }
}
