
using AutoMapper;
using SpredMedia.Authentication.Core.DTO;
using SpredMedia.Authentication.Model.model;

namespace SpredMedia.Authentication.Core.Utility
{
    public class AuthenticationProfile :Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<ClientRequestDto, Client>();
            CreateMap<Client, ClientResponseDto>()
                .ForMember(dest => dest.ClientId, act => act.MapFrom(src => src.Id));
            CreateMap<EndPoint, EndpointResponseDto>();
            CreateMap<EndpointRequestDto, EndPoint>();
            CreateMap<RegisterationDto, User>()
                .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.Email.ToLower()));
            CreateMap<User, RegisterationDto>();
        }
    }
}
