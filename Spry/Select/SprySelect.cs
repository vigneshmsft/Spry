using System;

namespace Spry.Select
{
    public sealed class SprySelect<TDto>
    {
        private SprySelectColumn<TDto> _column;
        private SprySelect()
        {

        }

        public static SprySelectColumn<TDto> Select()
        {
            var spry = new SprySelect<TDto>();
            return spry._column = new SprySelectColumn<TDto>(spry);
        }

        public string Build()
        {
            return "SELECT" + Environment.NewLine + _column.BuildImpl();
        }
    }
}
