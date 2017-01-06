using Spry.Select;

namespace Spry.Table
{
    public abstract class Join<TDto>
    {
        protected readonly SprySelect<TDto> Spry;
        protected readonly SprySelectTable<TDto> TableOne;
        protected readonly SprySelectTable<TDto> TableTwo;
        protected string OnCondition;

        protected Join(SprySelect<TDto> spry, SprySelectTable<TDto> tableOne, SprySelectTable<TDto> tableTwo)
        {
            TableTwo = tableTwo;
            TableOne = tableOne;
            Spry = spry;
        }

        public virtual SprySelectTable<TDto> On(string colOne, string colTwo)
        {
            OnCondition = " ON " + colOne + " = " + colTwo;
            return TableOne;
        }

        public virtual string Build()
        {
            return Spry.Build();
        }

        internal abstract string BuildImpl();
    }
}
