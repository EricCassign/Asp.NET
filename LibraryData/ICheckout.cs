using System;
using System.Collections.Generic;
using System.Text;
using LibraryData.Models;

namespace LibraryData
{
  public interface ICheckout
  {
    IEnumerable<Checkout> GetAll();
    Checkout GetById(int checkout);
    void Add(Checkout checkout);
    void CheckOutAsset(int assetId, int libraryCardId);
    IEnumerable<CheckoutHistory> GetCheckoutHistories(int id);
    Checkout GetLatestCheckout(int assetId);

    bool IsCheckedOut(int assetId);
    void PlaceHold(int assetId, int libraryCardId);
    Patron GetCurrentHoldPatron(int id);
    Patron GetCurrentCheckoutPatron(int assetId);
    DateTime GetCurrentHoldPlacedTime(int id);
    IEnumerable<Hold> GetCurrentHolds(int id);

    void CheckInItem(int assetId);
    void MarkLost(int assetId);
    void MarkFound(int assetId);
  }
}
