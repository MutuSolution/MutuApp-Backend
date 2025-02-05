using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Links;

public class LinkReportRequest
{
    public int LinkId { get; init; }
    public string Message { get; init; } = string.Empty;
}
