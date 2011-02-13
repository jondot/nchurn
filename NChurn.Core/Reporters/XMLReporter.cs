using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace NChurn.Core.Reporters
{
    public class XMLReporter : BaseAnalysisReporter
    {

        public XMLReporter(TextWriter outp) : base(outp)
        {
        }

        protected override void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns)
        {
            NChurnAnalysisResult xr = new NChurnAnalysisResult();
            xr.FileChurns = fileChurns.Select(
                x => new FileChurn { File = x.Key, Value = x.Value }).ToList();

            var ds = new DataContractSerializer(typeof(NChurnAnalysisResult));

            var settings = new XmlWriterSettings { Indent = true };

            using (var xw = XmlWriter.Create(_out, settings))
            {
                ds.WriteObject(xw, xr);
            }
        }
    }
    [DataContract]
    public class NChurnAnalysisResult
    {
        [DataMember]
        public List<FileChurn> FileChurns;
    }

    [DataContract]
    public class FileChurn
    {
        [DataMember]
        public string File;
        [DataMember]
        public int Value;
    }
}
