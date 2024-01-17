using DataAccess.Demo.DataAccessObject;
using DataAccess.Demo.Dbcontext;
using DataAccess.Demo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Demo.DataAcessObjecImpl
{
    public class ProductGenericRepository : GenericRepository<Product>, IProductGenericRepository
    {
        public ProductGenericRepository(MyShopDbContext dbContext) : base(dbContext)
        {
        }
    }
}
