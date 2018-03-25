using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
  public class PatronService : IPatron
  {
    private LibraryContext _context;

    public PatronService(LibraryContext context)
    {
      _context = context;
    }
    public Patron GetById(int id)
    {
      var patron = GetAll().FirstOrDefault(p => p.Id == id);
      return patron ?? null;
    }

    public IEnumerable<Patron> GetAll()
    {
      var patrons = _context.Patrons.Include(patron => patron.LibraryBranch).Include(patron => patron.LibraryCard);
      return patrons;
    }

    public void Add(Patron patron)
    {
      try
      {
        _context.Patrons.Add(patron);
        _context.SaveChanges();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    public IEnumerable<CheckoutHistory> GeCheckoutHistories(int patronId)
    {
      var libCardId = GetById(patronId).LibraryCard.Id;
      var histories = _context.CheckoutHistories.Include(ch => ch.LibraryCard).Include(ch => ch.LibraryAsset)
          .Where(ch => ch.LibraryCard.Id == libCardId).OrderByDescending(ch => ch.CheckedOut);
      return histories;
    }

    public IEnumerable<Hold> GetHolds(int patronId)
    {
      var libCardId = GetById(patronId).LibraryCard.Id;
      var holds = _context.Holds.Include(h => h.LibraryCard).Include(h => h.LibraryAsset)
          .Where(h => h.LibraryCard.Id == libCardId).OrderByDescending(h => h.HoldPlaced);
      return holds;
    }

    public IEnumerable<Checkout> GetCheckouts(int patronId)
    {
      var libCardId = GetById(patronId).LibraryCard.Id;
      var checkOuts = _context.Checkouts.Include(ch => ch.LibraryCard).Include(ch => ch.LibraryAsset)
          .Where(ch => ch.LibraryCard.Id == libCardId);
      return checkOuts;
    }

  }
}
