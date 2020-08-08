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
    public class ImportDataRepository : GenericRepositori<Tb_ImportExcelData>, IImportDataRepository
    {
        public ImportDataRepository(ApplicationDbContext Db) : base(Db)
        { }

        
    }
}
