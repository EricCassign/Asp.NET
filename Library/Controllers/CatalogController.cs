using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models.Catalog;
using Library.Models.CheckoutM;
using LibraryData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class CatalogController : Controller
    {
      private readonly ILibraryAsset _asset;
      private readonly ICheckout _checkout;

      public CatalogController(ILibraryAsset asset, ICheckout checkout)
      {

        _asset = asset;
        _checkout = checkout;
      }
        // GET: Catalog
        public ActionResult Index()
        {
          var assetModels = _asset.GetAll();
          var listingResult = assetModels.Select(
            result => new LibraryAssetDto
            {
              Id = result.Id,
              ImageUrl = result.ImageUrl,
              AuthorOrDirector = _asset.GetAuthorOrDirector(result.Id),
              Title=result.Title,
              Type = _asset.GetType(result.Id)
            });
          var model = new LibraryAssetApi()
          {
            Asset = listingResult
          };
            return View(model);
        }
    // GET: Catalog/Details/5
    public ActionResult Details(int id)
    {
      var asset = _asset.GetById(id) ;

      var  currentHolds =_checkout.GetCurrentHolds(id)
        .Select(a=>new AssetHoldModel()
        {
          Patron = _checkout.GetCurrentHoldPatron(a.Id),
          HolDateTime = _checkout.GetCurrentHoldPlacedTime(a.Id)
        });

      var model = new LibraryAssetDetailsDTO
      {
          Id = id,
          Title = asset.Title,
          Year = asset.Year,
          Cost = asset.Cost,
          AuthorOrDirector = _asset.GetAuthorOrDirector(id),
          ImageUrl = asset.ImageUrl,
          ISBN = _asset.GetIsbn(id),
          CurrentLocation = _asset.GetCurrentLibraryBranch(id).Name,
          Status = asset.Status.Name,
          NumberOfCopies = asset.NumberOfCopies,
          Type = _asset.GetType(id),
          Patron = _checkout.GetCurrentCheckoutPatron(id),
          CheckoutHistories = _checkout.GetCheckoutHistories(id),
          LatestCheckout = _checkout.GetLatestCheckout(id),
          CurrentHolds = currentHolds

      };
      //var model=new LibraryAssetDetailsApi
      //{
      //  LibraryAssetDetails = model
      //};
      return View(model);
    }
      public IActionResult Checkin(int id)
      {
        _checkout.CheckInItem(id);
        return RedirectToAction("Details", new { id = id });
      }


    public IActionResult Checkout(int id)
      {
        var asset = _asset.GetById(id);

        var model = new CheckoutDTO
        {
          AssetId = id,
          LibraryCardId = "",
          ImageUrl = asset.ImageUrl,
          Title = asset.Title,
          IsCheckeOut = _checkout.IsCheckedOut(id),
          HoldCount = _checkout.GetCurrentHolds(id).Count()
        };
        return View(model);
      }
      public IActionResult Hold(int id)
      {
        var asset = _asset.GetById(id);

        var model = new CheckoutDTO
        {
          AssetId = id,
          LibraryCardId = "",
          ImageUrl = asset.ImageUrl,
          Title = asset.Title,
          IsCheckeOut = _checkout.IsCheckedOut(id),
          HoldCount = _checkout.GetCurrentHolds(id).Count()
        };
        return View(model);
      }

    [HttpPost]
      public IActionResult PlaceCheckout(int assetId,int libraryCardId)
      {
      _checkout.CheckOutAsset(assetId, libraryCardId);
        return RedirectToAction("Details", new {id = assetId});
      }

      [HttpPost]
      public IActionResult PlaceHold(int assetId, int libraryCardId)
      {
        _checkout.PlaceHold(assetId, libraryCardId);
        return RedirectToAction("Details", new { id = assetId });
      }

      [HttpPost]
      public IActionResult MarkLost(int assetId)
      {
        _checkout.MarkLost(assetId);
        return RedirectToAction("Details", new { id = assetId });
      }

      [HttpPost]
      public IActionResult MarkFound(int assetId)
      {
        _checkout.MarkFound(assetId);
        return RedirectToAction("Details", new { id = assetId });
      }




    //// GET: Catalog/Create
    //public ActionResult Create()
    //{
    //    return View();
    //}

    //// POST: Catalog/Create
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Create(IFormCollection collection)
    //{
    //    try
    //    {
    //        // TODO: Add insert logic here

    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return View();
    //    }
    //}

    //// GET: Catalog/Edit/5
    //public ActionResult Edit(int id)
    //{
    //    return View();
    //}

    //// POST: Catalog/Edit/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Edit(int id, IFormCollection collection)
    //{
    //    try
    //    {
    //        // TODO: Add update logic here

    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return View();
    //    }
    //}

    //// GET: Catalog/Delete/5
    //public ActionResult Delete(int id)
    //{
    //    return View();
    //}

    //// POST: Catalog/Delete/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Delete(int id, IFormCollection collection)
    //{
    //    try
    //    {
    //        // TODO: Add delete logic here

    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch
    //    {
    //        return View();
    //    }
    //}

  }
}