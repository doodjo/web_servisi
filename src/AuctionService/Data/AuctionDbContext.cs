using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
        ///info za action 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// sve vezano za auctiondbcontext
    }

    public DbSet<Auction> Auctions { get; set; }
}