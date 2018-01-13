﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryData.Models
{
  public  class Book
    {
    [Required]
    public  string ISBN { get; set; }
    [Required]
    public  string Author { get; set; } 
   
    }
}
