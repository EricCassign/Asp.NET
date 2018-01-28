using LibraryData;
using System;
using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryServices
{
  public class LibraryAssetService: ILibraryAsset
   {
     private LibraryContext _context;

     public LibraryAssetService(LibraryContext context)
     {
       _context = context;
     }
      public IEnumerable<LibraryAsset> GetAll()
      {
    throw  new NotImplementedException();
   
      }

      public LibraryAsset GetById(int id)
      {
        throw new NotImplementedException();
      }

      public void Add(LibraryAsset libraryAsset)
      {

        _context.Add(libraryAsset);
      }

      public string GetAuthorOrDirector(int id)
      {
        throw new NotImplementedException();
      }

      public string GetTitle(int id)
      {
        throw new NotImplementedException();
      }

      public string GetISBN(int id)
      {
        throw new NotImplementedException();
      }

      public LibraryBranch GetCurrentLibraryBranch(int id)
      {
        throw new NotImplementedException();
      }
    }
}
