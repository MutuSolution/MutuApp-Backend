using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public class LinkReport
{
    public int Id { get; set; }
    public int LinkId { get; set; }
    public string? Message { get; set; }
    public bool? IsPermitted { get; set; }
}
