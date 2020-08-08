using DAL;
using DAL.Models;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class SettingRepository : GenericRepositori<Tb_Setting>, ISettingRepository
    {
        public SettingRepository(ApplicationDbContext Db) : base(Db)
        { }


    }
}
