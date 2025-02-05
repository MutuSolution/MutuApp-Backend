using AutoMapper;
using Common.Requests.Links;
using Common.Responses.Links;
using Domain;

namespace Application;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateLinkRequest, Link>();
        CreateMap<LinkReportRequest, LinkReport>();
        CreateMap<Link, LinkResponse>().ReverseMap();
        CreateMap<Like, LikeResponse>().ReverseMap();
        CreateMap<Like, LinkResponse>().ReverseMap();
        CreateMap<LinkReport, LinkReportResponse>().ReverseMap();

    }
}
