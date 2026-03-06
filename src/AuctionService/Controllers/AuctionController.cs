using System.Runtime.CompilerServices;
using AuctionSerivce.DTOs;
using AuctionService.Data;
using AuctionService.Entities;
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

    // post request
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = mapper.Map<Auction>(auctionDto);
        // todo add current user as seller

        auction.Seller = "test";

        context.Auctions.Add(auction);

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to db");

        return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, mapper.Map<AuctionDto>(auction));
    }

    //put request
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        //todo check seller username

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Colour = updateAuctionDto.Colour ?? auction.Item.Colour;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        
        var result = await context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Updating problem kod AuctionController za UpdateAuctionDto");

    }

    // delete request
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await context.Auctions.FindAsync(id);

        if (auction == null) return NotFound();

        context.Auctions.Remove(auction);

         var result = await context.SaveChangesAsync() > 0;

         if (!result) return BadRequest("Could not update the db");

         return Ok();

    }
}