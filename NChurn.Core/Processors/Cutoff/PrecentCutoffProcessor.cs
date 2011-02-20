using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NChurn.Core.Processors.Cutoff
{
    public class PrecentCutoffProcessor : IProcessor<KeyValuePair<string,int>>
    {
        private float _precent;

        public PrecentCutoffProcessor(float precent)
        {
            _precent = precent;
        }
        public IEnumerable<KeyValuePair<string, int>> Apply(IEnumerable<KeyValuePair<string, int>> input)
        {
            //quick n dirty.
            var sum = input.Sum(x => x.Value);
            var tt = sum * _precent;
            var count = 0;
            var tempsum = 0;
            foreach (var keyValuePair in input)
            {
                tempsum += keyValuePair.Value;
                if (tempsum > tt)
                    break;
                count++;
            }
            return input.Take(count);
        }
    }
}
