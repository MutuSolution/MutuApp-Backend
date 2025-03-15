using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.Links;

public class LinkReportResponse
{
    public int Id { get; init; }
    public int LinkId { get; init; }
    public string? Message { get; init; }
    public bool? IsChecked { get; init; }
}
