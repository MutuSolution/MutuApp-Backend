using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Links.Report;

public class LinkReporIsPermittedRequest
{
    public int ReportId { get; set; }
    public bool IsPermitted { get; set; }
}
