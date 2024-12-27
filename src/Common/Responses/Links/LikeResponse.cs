using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.Links;
public class LikeResponse
{
    public string LikeId { get; set; }
    public string LinkId { get; set; }
    public bool IsLiked { get; set; }
}
