using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.Dbcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.UnitOfWork
{
    public class MyShopUnitOfWork : IMyShopUnitOfWork
    {
        // public IProductRepository productRepository { get; set; }
       public IAccountRepository accountRepository { get; set; }

        public IProductGenericRepository _productGenericRepository { get; set; }
        
        private MyShopDbContext _dbcontext;

        //public MyShopUnitOfWork(MyShopDbContext dbcontext, IProductRepository productRepository, IAccountRepository accountRepository)
        //{
        //    _dbcontext = dbcontext;
        //    this.productRepository = productRepository;
        //    this.accountRepository = accountRepository;
        //}

     
        public MyShopUnitOfWork(MyShopDbContext dbcontext, IProductGenericRepository productGenericRepository, IAccountRepository accountRepository)
        {
            _dbcontext = dbcontext;
            this._productGenericRepository = productGenericRepository;
            this.accountRepository = accountRepository;
        }

        public int SaveChange()
        {
           return _dbcontext.SaveChanges();
        }
    }
}
