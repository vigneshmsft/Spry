using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public class SpryParameters : IEnumerable<KeyValuePair<string, object>>
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

        public void Add(SpryParameters parameters)
        {
            if (parameters != null && parameters._parameters.Any())
            {
                _parameters.AddRange(parameters._parameters);
            }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var spryParameter in _parameters)
            {
                yield return new KeyValuePair<string, object>(spryParameter.Name, spryParameter.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
