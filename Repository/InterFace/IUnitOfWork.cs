using DAL.Models;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.InterFace
{
    public interface IUnitOfWork
    {
        public SellRepository SellRepo { get; }

        public UserRepository UserRepo { get; }

        public TeamRepository TeamRepo { get; }

        public AgentRepository AgentRepo { get; }

        public FactorRepository FactorRepo { get; }        

        public BrokerRepository BrokerRepo { get; }

        public ProductRepository ProductRepo { get; }

        public SupportRepository SupportRepo { get; }

        public SettingRepository SettingRepo { get; }        
        
        public AffiliateRepository AffiliateRepo { get; }

        public TeamUsersRepository TeamUsersRepo { get; }        

        public ImportDataRepository ImportDataRepo { get; }

        public SupportTypeRepository SupportTypeRepo { get; }

        public ProductDetailRepository ProductDetailRepo { get; }

        public BusinessOwnerRepository BusinessOwnerRepo { get; }

        public AffiliateReportRepository AffiliateReportRepo { get; }

        public AffiliateParameterRepository AffiliateParameterRepo { get; }

        bool BackUpFromDb(string path);

        void Save();
        Task SaveAsync();
    }
}
