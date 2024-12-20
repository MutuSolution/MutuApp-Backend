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
        CreateMap<Link, LinkResponse>();
    }
}
