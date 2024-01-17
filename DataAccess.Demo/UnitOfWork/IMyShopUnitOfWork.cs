using DataAccess.Demo.DataAccessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.UnitOfWork
{
    public interface IMyShopUnitOfWork
    {
        //public IProductRepository productRepository { get; set; }
        public IAccountRepository accountRepository { get; set; }
        public IProductGenericRepository _productGenericRepository { get; set; }
        int SaveChange();
    }
}
