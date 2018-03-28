using System;
using System.Collections.Generic;
using System.Text;
using LibraryData.Models;

namespace LibraryData
{
    public interface IPatron
    {
      Patron GetById(int id);
      IEnumerable<Patron> GetAll();
      void Add(Patron patron);

      IEnumerable<CheckoutHistory> GeCheckoutHistories(int patronId);
      IEnumerable<Hold> GetHolds(int patronId);
      IEnumerable<Checkout> GetCheckouts(int patronId);
    }
}
