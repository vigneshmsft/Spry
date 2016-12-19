using System.Collections.Generic;

namespace Spry
{
    public class SpryParameter
    {
        public SpryParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public object Value { get; set; }
    }

    public class SpryParameters
    {
        private readonly List<SpryParameter> _parameters = new List<SpryParameter>();

        public SpryParameters Add(string name, object value)
        {
            _parameters.Add(new SpryParameter(name, value));
            return this;
        }

        public SpryParameters Add(SpryParameter parameter)
        {
            _parameters.Add(parameter);
            return this;
        }

        public IEnumerable<SpryParameter> Parameters
        {
            get { return _parameters; }
        }
    }
}
