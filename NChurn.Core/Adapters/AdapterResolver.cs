using System.IO;
using NChurn.Core.Adapters.Git;
using NChurn.Core.Adapters.Svn;
using NChurn.Core.Adapters.TF;

namespace NChurn.Core.Adapters
{
    internal class AdapterResolver : IAdapterResolver
    {
        public IVersioningAdapter CreateAdapter()
        {

            if(Directory.Exists(".git"))
            {
                return new GitAdapter();
            }
            if(Directory.Exists(".svn"))
            {
                return new SvnAdapter();
            }

            return new TFAdapter();
        }
    }
}