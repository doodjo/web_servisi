using System.ComponentModel.DataAnnotations;

namespace AuctionSerivce.DTOs;

public class CreateAuctionDto
{
    // data annotation
    [Required]
    public string Make { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public string Colour { get; set; }
    [Required]
    public int Mileage { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public int ReservePrice { get; set; }
    [Required]
    public DateTime AuctionEnd { get; set; }
}