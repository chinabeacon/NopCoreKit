using System;
using System.Collections.Generic;
using System.Text;

namespace ChinaBeacon.Sdk.Core.Configuration
{
    public class WebsiteConfig
    {
        public string LawEnforcementApiUri { get; set; }
        public string MajorHazardApiUri { get; set; }
        public string ProgramName { get; set; }
        public string GetS3ServiceEndPoint { get; set; }
    }
}
