using DAL;
using DAL.Models;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ProductRepository : GenericRepositori<Tb_Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext Db) : base(Db)
        { }


    }
}
