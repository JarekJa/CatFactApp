using CatFactApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatFactApp.Services.Interfaces
{
    public interface IFileService
    {
        Task<bool> AppendCatFactAsync(CatFactResponse catFact);
    }
}
