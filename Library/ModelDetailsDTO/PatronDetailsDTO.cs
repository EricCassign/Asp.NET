using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData.Models;

namespace Library.Models
{
  public class PatronDetailsDTO
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => FirstName + " " + LastName;
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string DateOfBirthString => FormatDate(DateOfBirth);
    public DateTime MemberSince { get; set; }
    public string MemberSinceString => FormatDate(MemberSince);
    public string PhoneNumber { get; set; }
    public int LibraryCardId { get; set; }
    public decimal OverdueFees { get; set; }
    public string LibraryBranch { get; set; }
    public string ImageUrl { get; set; }

    public IEnumerable<CheckoutHistory> CheckoutHistories { get; set; }
    public IEnumerable<Checkout> Checkouts { get; set; }
    public IEnumerable<Hold> Holds { get; set; }

    public string FormatDate(DateTime dateTime)
    {
      return dateTime.ToString("dd-MMMM-yyyy");
    }
  }

  public class PatronApi
  {
    public IEnumerable<PatronDetailsDTO> Patron { get; set; }
  }

 
}
