using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NChurn.Core.Adapters;

namespace NChurn.Core.Analyzers
{
    public class Analyzer
    {
        private readonly IVersioningAdapter _adapter;
        private readonly List<Regex> _includes = new List<Regex>();
        private readonly List<Regex> _excludes = new List<Regex>();


        public static Analyzer Create()
        {
            return new Analyzer(new AutoDiscoveryAdapter());
        }

        public static Analyzer Create(IVersioningAdapter adapter)
        {
            return new Analyzer(adapter);
        }

        internal Analyzer(IVersioningAdapter adapter)
        {
            _adapter = adapter;
        }

        public AnalysisResult Analyze(string input)
        {
            IEnumerable<string> changedResources = _adapter.Parse(input);
            return AnalyzeChangedResources(changedResources);
        }

        public AnalysisResult Analyze(DateTime backTo)
        {
            IEnumerable<string> changedResources = GetChangedResources(backTo);
            return AnalyzeChangedResources(changedResources);
        }
        public AnalysisResult Analyze()
        {
            IEnumerable<string> changedResources = GetChangedResources();
            return AnalyzeChangedResources(changedResources);
        }

        private AnalysisResult AnalyzeChangedResources(IEnumerable<string> changedResources)
        {
            var d = new Dictionary<string, int>();

            foreach (var x in ApplyExcludeIncludes(changedResources))
            {
                if (!d.ContainsKey(x)) d[x] = 0; d[x]++;
            }
            return new AnalysisResult {FileChurn = d.OrderByDescending(x=>x.Value).ThenBy( x=>x.Key)};
        }

        private IEnumerable<string> ApplyExcludeIncludes(IEnumerable<string> changedResources)
        {
            if (_includes.Count == 0 && _excludes.Count == 0)
                return changedResources;

            return changedResources.Where(x => (_excludes.Count == 0 || _excludes.All(y => !y.IsMatch(x)) )
                                               &&  (_includes.Count == 0 || _includes.Any(y => y.IsMatch(x))));
                                    
        }

        private IEnumerable<string> GetChangedResources()
        {
            return _adapter.ChangedResources();
        }

        private IEnumerable<string> GetChangedResources(DateTime backTo)
        {
            return _adapter.ChangedResources(backTo);
        }

        public void AddInclude(string pattern)
        {
            _includes.Add(new Regex(pattern, RegexOptions.Compiled));
        }

        public void AddExcludes(params string[] patterns)
        {
            foreach (var pattern in patterns)
            {
                _excludes.Add(new Regex(pattern, RegexOptions.Compiled));
            }
        }
    }
}


//
// .net 40 analyzer map/reduce. taken out because of tools not being ready / hacky (ncover, specifying framework dir in parameters)
//

/*
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NChurn.Core.Adapters;

namespace NChurn.Core.Analyzers
{
    public class Analyzer
    {
        private readonly IAdapterResolver _adapterResolver;

        public static Analyzer Create()
        {
            //todo: ioc resolve
            return new Analyzer(new AdapterResolver());
        }
        

        internal Analyzer(IAdapterResolver adapterResolver)
        {
            _adapterResolver = adapterResolver;
        }

        public AnalysisResult Analyze(DateTime backTo)
        {
            IEnumerable<string> changedResources = GetChangedResources(backTo);

            return AnalyzeChangedResources(changedResources);
        }
        public AnalysisResult Analyze()
        {
            IEnumerable<string> changedResources = GetChangedResources();

            return AnalyzeChangedResources(changedResources);
        }

        private static AnalysisResult AnalyzeChangedResources(IEnumerable<string> changedResources)
        {
            var concurrentDictionary = new ConcurrentDictionary<Guid, Dictionary<string, int>>();

            Parallel.ForEach(Partitioner.Create(changedResources), () => new Dictionary<string, int>(),
                             (x, s, i, d) => { if (!d.ContainsKey(x)) d[x] = 0; d[x]++; return d; }, x => concurrentDictionary[Guid.NewGuid()] = x);


            var r = new Dictionary<string, int>();
            Reduce(concurrentDictionary, r);
            return new AnalysisResult {FileChurn = r.OrderByDescending(x=>x.Value).ThenBy( x=>x.Key)};
        }

        private IEnumerable<string> GetChangedResources()
        {
            IVersioningAdapter versioningAdapter = _adapterResolver.CreateAdapter();
            return versioningAdapter.ChangedResources();
        }

        private IEnumerable<string> GetChangedResources(DateTime backTo)
        {
            IVersioningAdapter versioningAdapter = _adapterResolver.CreateAdapter();
            return versioningAdapter.ChangedResources(backTo);
        }

        private static void Reduce(ConcurrentDictionary<Guid, Dictionary<string, int>> concurrentDictionary, Dictionary<string, int> r)
        {
            foreach (var d in concurrentDictionary)
            {
                MergeResult(d.Value, r);
            }
        }

        private static void MergeResult(Dictionary<string, int> d, Dictionary<string, int> r)
        {
            foreach (var kp in d)
            {
                if (!r.ContainsKey(kp.Key))
                    r[kp.Key] = 0;
                r[kp.Key] = r[kp.Key] + kp.Value;
            }
        }
    }
}
*/