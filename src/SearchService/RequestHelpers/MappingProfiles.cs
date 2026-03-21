using AutoMapper;
using Contracts;
using MassTransit;

namespace SearchService;


public class MappingProfiles : Profile
{

    public MappingProfiles()
    {
        CreateMap<AuctionCreated, Item>();
    }
}

