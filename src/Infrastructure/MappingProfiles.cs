using AutoMapper;
using Common.Responses.Identity;
using Common.Responses.Links;
using Domain;
using Infrastructure.Models;

namespace Infrastructure;

internal class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ApplicationUser, UserResponse>();
        CreateMap<ApplicationRole, RoleResponse>();
        CreateMap<Link, LikeResponse>();
        CreateMap<LinkReport, LinkReportResponse>().ReverseMap();
        CreateMap<ApplicationRoleClaim, RoleClaimViewModel>().ReverseMap();
    }
}
