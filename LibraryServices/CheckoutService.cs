using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{

  public class CheckoutService : ICheckout
  {
    private LibraryContext _context;
    DateTime _now = DateTime.Now;

    public CheckoutService(LibraryContext context)
    {
      _context = context;
    }
    public IEnumerable<Checkout> GetAll()
    {
      return _context.Checkouts;
    }

    public Checkout GetById(int id)
    {
      return GetAll().FirstOrDefault(checkout => checkout.Id == id);
    }

    public void Add(Checkout checkout)
    {
      _context.Checkouts.Add(checkout);
      _context.SaveChanges();
    }

    public void CheckOutAsset(int assetId, int libraryCardId)
    {
      if (IsCheckedOut(assetId))
      {
        return;
      }
      var item = _context.LibraryAssets.FirstOrDefault(asset => asset.Id == assetId);
      UpdateAssetStatus(assetId, "Checked Out");
      var librayCard = _context
        .LibraryCards.Include(card => card.Checkouts)
        .FirstOrDefault(card => card.Id == libraryCardId);

      var checkOut = new Checkout
      {
        LibraryAsset = item,
        LibraryCard = librayCard,
        Since = _now,
        Until = GetDefaultCheckoutTime(_now)
      };
      _context.Add(checkOut);
      var checkoutHistory = new CheckoutHistory
      {
        LibraryAsset = item,
        LibraryCard = librayCard,
        CheckedOut = _now
      };
      _context.Add(checkoutHistory);
      _context.SaveChanges();

    }

    private DateTime GetDefaultCheckoutTime(DateTime now)
    {
      return now.AddDays(30);
    }

    public bool IsCheckedOut(int assetId)
    {
      var isCheckedOut = _context.Checkouts.Any(asset => asset.LibraryAsset.Id == assetId);
      return isCheckedOut;
    }

    public IEnumerable<CheckoutHistory> GetCheckoutHistories(int id)
    {
      return _context.CheckoutHistories
      .Include(h => h.LibraryAsset)
      .Include(h => h.LibraryCard)
      .Where(h => h.LibraryAsset.Id == id);
    }

    public Checkout GetLatestCheckout(int assetId)
    {
      return _context.Checkouts.Where(c => c.LibraryAsset.Id == assetId)
        .OrderByDescending(c => c.Since)
        .FirstOrDefault();
    }

    public void PlaceHold(int assetId, int libraryCardId)
    {
      var asset = _context.LibraryAssets
        .Include(s=>s.Status)
        .FirstOrDefault(a => a.Id == assetId);
      var card = _context.LibraryCards.FirstOrDefault(l => l.Id == libraryCardId);
      if (asset.Status.Name == "Available")
      {
        UpdateAssetStatus(assetId, "On Hold");
      }
      var hold = new Hold
      {
        HoldPlaced = _now,
        LibraryAsset = asset,
        LibraryCard = card
      };
      _context.Add(hold);
      _context.SaveChanges();
    }


    public Patron GetCurrentHoldPatron(int holdId)
    {
      var hold = _context.Holds.Include(la => la.LibraryAsset).Include(lc => lc.LibraryCard)
        .FirstOrDefault(h => h.Id == holdId);
      if (hold == null) return null;
      {
        var cardId = hold.LibraryCard.Id;
        var patron = _context.Patrons.Include(lc => lc.LibraryCard).FirstOrDefault(p => p.LibraryCard.Id == cardId);
        return patron;
      }
    }

    public Patron GetCurrentCheckoutPatron(int assetId)
    {
      var checkout = _context.Checkouts.Include(lc => lc.LibraryCard)
        .Include(la => la.LibraryAsset)
        .FirstOrDefault(c => c.LibraryAsset.Id == assetId);
      if (checkout == null) return null;
      var cardId = checkout.LibraryCard.Id;
      var patron = _context.Patrons.Include(lc => lc.LibraryCard).FirstOrDefault(p => p.LibraryCard.Id == cardId);
      return patron;
    }

    public DateTime GetCurrentHoldPlacedTime(int id)
    {
      var hold = _context.Holds.FirstOrDefault(h => h.Id == id);
      if (hold != null)
      {
        return hold.HoldPlaced;
      }
      return _now;
    }

    public IEnumerable<Hold> GetCurrentHolds(int id)
    {
      return _context.Holds
      .Include(h => h.LibraryAsset)
      .Where(h => h.LibraryAsset.Id == id);
    }

    public void CheckInItem(int assetId)
    {
      var item = _context.LibraryAssets.FirstOrDefault(asset => asset.Id == assetId);
      //_context.Update(item);
      //remove existing checkouts
      RemoveExistingCheckouts(assetId);
      //close existing checkput history
      CloseExistingCheckouts(assetId, _now);
      //look for existing hold
      var currentHolds = _context.Holds
        .Include(h => h.LibraryAsset)
        .Include(h => h.LibraryCard)
        .Where(h => h.LibraryAsset.Id == assetId);
      //checkout  earliest hold
      if (currentHolds.Any())
      {
        var earliestHold = currentHolds.OrderBy(holds => holds.HoldPlaced).FirstOrDefault();
        if (earliestHold != null)
        {
          var card = earliestHold.LibraryCard;
          _context.Remove(earliestHold);
          _context.SaveChanges();
          CheckOutAsset(assetId, card.Id);
        }
      }
      else
      {
        //set item status available
        UpdateAssetStatus(assetId, "Available");
      }

      _context.SaveChanges();
    }

    public void MarkLost(int assetId)
    {
      UpdateAssetStatus(assetId, "Lost");
      _context.SaveChanges();
    }

    public void MarkFound(int assetId)
    {
      UpdateAssetStatus(assetId, "Available");

      //remove existing checkouts
      RemoveExistingCheckouts(assetId);

      //close existing checkout history
      CloseExistingCheckouts(assetId, _now);

      _context.SaveChanges();
    }

    private void CloseExistingCheckouts(int assetId, DateTime dateTime)
    {
      var history =
        _context.CheckoutHistories.FirstOrDefault(h => h.LibraryAsset.Id == assetId && h.CheckedIn == null);
      if (history != null)
      {
        _context.Update(history);
        history.CheckedIn = dateTime;
      }
    }

    private void RemoveExistingCheckouts(int assetId)
    {
      var checkout = _context.Checkouts.FirstOrDefault(c => c.LibraryAsset.Id == assetId);
      if (checkout != null)
      {
        _context.Remove(checkout);
      }
    }

    public void UpdateAssetStatus(int assetId, string statusString)
    {
      var item = _context.LibraryAssets.FirstOrDefault(asset => asset.Id == assetId);
      if (item != null)
      {
        _context.Update(item);
        item.Status = _context.Statuses.FirstOrDefault(status => status.Name == statusString);
      }

    }
  }
}
