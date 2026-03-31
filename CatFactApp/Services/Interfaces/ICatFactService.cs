using CatFactApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatFactApp.Services.Interfaces
{
    internal interface ICatFactService
    {
        Task<CatFactResponse?> GetCatFactAsync();
    }
}
