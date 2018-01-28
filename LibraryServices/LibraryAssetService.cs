using LibraryData;
using System;
using System.Collections.Generic;
using System.Linq;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
  public class LibraryAssetService : ILibraryAsset
  {
    private readonly LibraryContext _context;

    public LibraryAssetService(LibraryContext context)
    {
      _context = context;
    }
    public IEnumerable<LibraryAsset> GetAll()
    {
      var assets = _context.LibraryAssets
        .Include(asset => asset.Status)
        .Include(asset => asset.Location);

      return assets;

    }

    public LibraryAsset GetById(int id)
    {
      var asset = GetAll().FirstOrDefault(a => a.Id == id);
      return asset;
    }

    public void Add(LibraryAsset libraryAsset)
    {

      _context.Add(libraryAsset);
      _context.SaveChanges();
    }

    public string GetAuthorOrDirector(int id)
    {
      var isBook = _context.LibraryAssets
        .OfType<Book>().Any(asset => asset.Id == id);
      //var isVideo = _context.LibraryAssets
      //  .OfType<Video>().Any(asset => asset.Id == id);

      return isBook
        ? _context.Books.FirstOrDefault(book => book.Id == id)?.Author
        : _context.Videos.FirstOrDefault(video => video.Id == id)?.Director
          ?? "Unknown";
    }

    public string GetTitle(int id)
    {
      var title = GetById(id).Title;
      return title;
    }

    public string GetIsbn(int id)
    {
      if (_context.Books.Any(a => a.Id == id))
      {
        return _context.Books.FirstOrDefault(book => book.Id == id)?.Isbn;
      }
      return "";
    }

    public LibraryBranch GetCurrentLibraryBranch(int id)
    {
      var location = GetById(id).Location;
      return location;
    }

    public string GetType(int id)
    {
      var book = _context.LibraryAssets.OfType<Book>()
        .Where(b => b.Id == id);
      return book.Any() ? "Book" : "Video";
    }
  }
}
