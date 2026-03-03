using AuctionSerivce.DTOs;
using AuctionService.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionSerivce.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController: ControllerBase
{

    // DEPENDECNY INJECTIONS 
    // kada se zatrazi nova instanca ovog AuctionsControllera, (kada naidje neki HTTP zahtev), pogledaju se argumenti (db context i mapper) 
    // ima da se instanciraju te klase i da postanu dostupne za red sa samo tom instancom. Ovo omogucava da se AuctionDbContext i IMapper
    // samo privremeno "INJECT-uj" kako bi bile na usluzi datoj instanci dok je ziva
    
    private readonly AuctionDbContext context;
    private readonly IMapper mapper;

    public AuctionController(AuctionDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }


    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await context.Auctions
        .Include(x => x.Item)
        .OrderBy(x => x.Item.Make)
        .ToListAsync();

        return mapper.Map<List<AuctionDto>>(auctions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await context.Auctions
        .Include(x => x.Item)
        .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        return mapper.Map<AuctionDto>(auction);
    }
}