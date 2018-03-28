using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using LibraryData;
using LibraryData.Models;

namespace Library.Controllers
{
  public class PatronController : Controller
  {
    private IPatron _patron;

    public PatronController(IPatron patron)
    {
      _patron = patron;
    }
    // GET: Patron
    public IActionResult Index()
    {
      var patrons = _patron.GetAll();
      if (patrons == null)
      {
        return NotFound();
      }
      var patronModel = patrons.Select(p => new PatronDetailsDTO
      {
        Id = p.Id,
        Address = p.Address,
        PhoneNumber = p.PhoneNumber,
        FirstName = p.FirstName,
        LastName = p.LastName,
        OverdueFees = p.LibraryCard.Fees,
        LibraryBranch = p.LibraryBranch.Name,
        LibraryCardId = p.LibraryCard.Id
      }).ToList();

      var model = new PatronApi()
      {
        Patron = patronModel
      };
      return View(model);
    }

    public IActionResult Details(int id)
    {
      var patron = _patron.GetById(id);
      if (patron == null)
      {
        return NotFound();
      }
      var model = new PatronDetailsDTO()
      {
        FirstName = patron.FirstName,
        Address = patron.Address,
        DateOfBirth = patron.DateOfBirth,
        LastName = patron.LastName,
        OverdueFees = patron.LibraryCard.Fees,
        MemberSince = patron.LibraryCard.Created,
        LibraryCardId = patron.LibraryCard.Id,
        PhoneNumber = patron.PhoneNumber,
        LibraryBranch = patron.LibraryBranch.Name,
        ImageUrl = patron.ImageUrl,
        Checkouts = _patron.GetCheckouts(id).ToList() ?? new List<Checkout>(),
        CheckoutHistories = _patron.GeCheckoutHistories(id).ToList() ?? new List<CheckoutHistory>(),
        Holds = _patron.GetHolds(id).ToList() ?? new List<Hold>()

      };
      return View(model);
    }



    //// GET: Patron/Create
    //public IActionResult Create()
    //{
    //    return View();
    //}

    //// POST: Patron/Create
    //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Address,DateOfBirth,PhoneNumber")] Patron patron)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _context.Add(patron);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(patron);
    //}

    //// GET: Patron/Edit/5
    //public async Task<IActionResult> Edit(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var patron = await _context.Patrons.SingleOrDefaultAsync(m => m.Id == id);
    //    if (patron == null)
    //    {
    //        return NotFound();
    //    }
    //    return View(patron);
    //}

    //// POST: Patron/Edit/5
    //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,DateOfBirth,PhoneNumber")] Patron patron)
    //{
    //    if (id != patron.Id)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            _context.Update(patron);
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!PatronExists(patron.Id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(patron);
    //}

    //// GET: Patron/Delete/5
    //public async Task<IActionResult> Delete(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var patron = await _context.Patrons
    //        .SingleOrDefaultAsync(m => m.Id == id);
    //    if (patron == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(patron);
    //}

    //// POST: Patron/Delete/5 
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    var patron = await _context.Patrons.SingleOrDefaultAsync(m => m.Id == id);
    //    _context.Patrons.Remove(patron);
    //    await _context.SaveChangesAsync();
    //    return RedirectToAction(nameof(Index));
    //}

    //private bool PatronExists(int id)
    //{
    //    return _context.Patrons.Any(e => e.Id == id);
    //}
  }
}
