namespace Library.Models
{
    public class CheckoutDetailsDTO
    {
    public string LibraryCardId { get; set; }
    public string Title { get; set; }
    public int AssetId { get; set; }
    public  string ImageUrl { get; set; }
    public int HoldCount { get; set; }
    public bool IsCheckeOut { get; set; }
    }
}
 