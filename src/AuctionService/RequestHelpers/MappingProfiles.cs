using AuctionSerivce.DTOs;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionSerivce;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // auction na auctionDTO 
        //uzimam iz Auction atribute i povezujem ih na AuctionDto ali Auction ima Item surogat kljuc Item tako da vuce i iz tabele Items
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        //mora da zna kako da te Items mapira pa kazemo da Item ide na AuctionDto jer se sve kulminise u AuctionDto
        CreateMap<Item, AuctionDto>(); //item na auctionDto
        //mapira CrateAuctionDto na osnovu Auction klase ali za Item popuni tako sto koristis ceo source objekat CreateAuctionDto i povezi ga na Auction
        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateAuctionDto, Item>(); //automapper zna da poveze i napravi Item iz CreateAuctionDto
    }
}