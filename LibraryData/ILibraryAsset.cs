using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
  public interface ILibraryAsset
  {
    IEnumerable<LibraryAsset> GetAll();
    LibraryAsset GetById(int id);
    void Add(LibraryAsset libraryAsset);
    string GetAuthorOrDirector(int id);
    string GetTitle(int id);
    string GetIsbn(int id);
    string GetType(int id);

    LibraryBranch GetCurrentLibraryBranch(int id);

  }
}
