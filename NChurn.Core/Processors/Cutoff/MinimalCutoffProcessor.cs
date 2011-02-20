using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NChurn.Core.Processors.Cutoff
{
    public class MinimalCutoffProcessor : IProcessor<KeyValuePair<string,int>>
    {
        private readonly int _minimum;
        public MinimalCutoffProcessor(int minimum)
        {
            _minimum = minimum;
        }
        public IEnumerable<KeyValuePair<string, int>> Apply(IEnumerable<KeyValuePair<string, int>> input)
        {
            return input.Where(x => x.Value > _minimum);
        }
    }
}
