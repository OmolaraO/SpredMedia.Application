using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.CommonLibrary
{
    public interface IGenericUnitOfWork
    {
        Task Commit();
        Task CreateTransaction();
        void Dispose();
        Task Rollback();
        Task Save();
    }
}
