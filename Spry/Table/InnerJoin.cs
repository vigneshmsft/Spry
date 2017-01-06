using System;
using Spry.Select;

namespace Spry.Table
{
    public class InnerJoin<TDto> : Join<TDto>
    {
        public InnerJoin(SprySelect<TDto> spry, SprySelectTable<TDto> tableOne, SprySelectTable<TDto> tableTwo)
            : base(spry, tableOne, tableTwo)
        {

        }

        internal override string BuildImpl()
        {
            return "INNER JOIN " + TableTwo.BuildImpl() + OnCondition + Environment.NewLine;
        }
    }
}
