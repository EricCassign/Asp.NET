using System;
using System.Collections.Generic;
using LibraryData.Models;

namespace Library.Models.Catalog
{
  public class LibraryAssetDetailsDTO
  {
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string Title { get; set; }
    public string AuthorOrDirector { get; set; }
    public string Type { get; set; }
    public int NumberOfCopies { get; set; }
    public int Year { get; set; }
    public string ISBN { get; set; }
    public decimal Cost { get; set; }
    public string CurrentLocation { get; set; }
    public  string Status { get; set; }
    public Patron Patron { get; set; }
    public Checkout LatestCheckout { get; set; }
    public IEnumerable<CheckoutHistory> CheckoutHistories { get; set; }
    public IEnumerable<AssetHoldModel> CurrentHolds { get; set; }
  }

  public class AssetHoldModel
  {
    public string PatronName { get; set; }
    public DateTime HolDateTime { get; set; }
  }
  
}
