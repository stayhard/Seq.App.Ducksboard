using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seq.Apps;

namespace Seq.App.Ducksboard
{
    public abstract class DucksboardBaseReactor : Reactor
    {
        [SeqAppSetting(
            DisplayName = "API Key")]
        public string ApiKey { get; set; }

        [SeqAppSetting(
            DisplayName = "Data Source Label",
            HelpText = "Get this from widget preferences.")]
        public string DataLabel { get; set; }
    }
}
