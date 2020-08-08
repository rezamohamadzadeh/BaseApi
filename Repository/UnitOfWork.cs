using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.InterFace;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private UserRepository _UserRepo;
        public UserRepository UserRepo
        {
            get
            {
                if (_UserRepo == null)
                {
                    _UserRepo = new UserRepository(_dbContext);
                }
                return _UserRepo;
            }
        }


        private TeamRepository _TeamRepo;

        /// <summary>
        /// Team Repository
        /// </summary>
        /// <returns></returns>
        public TeamRepository TeamRepo
        {
            get
            {
                if (_TeamRepo == null)
                {
                    _TeamRepo = new TeamRepository(_dbContext);
                }
                return _TeamRepo;
            }
        }

        private TeamUsersRepository _TeamUsersRepo;


        /// <summary>
        /// TeamUsers Repository
        /// </summary>
        /// <returns></returns>
        public TeamUsersRepository TeamUsersRepo
        {
            get
            {
                if (_TeamUsersRepo == null)
                {
                    _TeamUsersRepo = new TeamUsersRepository(_dbContext);
                }
                return _TeamUsersRepo;
            }
        }


        private ImportDataRepository _importDataRepository;

        /// <summary>
        /// For Test Import Excel To Db
        /// </summary>
        /// <returns></returns>
        public ImportDataRepository ImportDataRepo
        {
            get
            {
                if (_importDataRepository == null)
                {
                    _importDataRepository = new ImportDataRepository(_dbContext);
                }
                return _importDataRepository;
            }
        }


        private AffiliateReportRepository _affiliateReportRepo;
        /// <summary>
        /// Affiliate ReportRepository
        /// </summary>
        /// <returns></returns>
        public AffiliateReportRepository AffiliateReportRepo
        {
            get
            {
                if (_affiliateReportRepo == null)
                {
                    _affiliateReportRepo = new AffiliateReportRepository(_dbContext);
                }
                return _affiliateReportRepo;
            }
        }

        private ProductRepository _productRepository;

        public ProductRepository ProductRepo
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_dbContext);
                }
                return _productRepository;
            }
        }

        private SettingRepository _settingRepository;

        public SettingRepository SettingRepo
        {
            get
            {
                if (_settingRepository == null)
                {
                    _settingRepository = new SettingRepository(_dbContext);
                }
                return _settingRepository;
            }
        }

        private BusinessOwnerRepository _ownerRepository;

        public BusinessOwnerRepository BusinessOwnerRepo
        {
            get
            {
                if (_ownerRepository == null)
                {
                    _ownerRepository = new BusinessOwnerRepository(_dbContext);
                }
                return _ownerRepository;
            }
        }

        private BrokerRepository _brokerRepository;

        public BrokerRepository BrokerRepo
        {
            get
            {
                if (_brokerRepository == null)
                {
                    _brokerRepository = new BrokerRepository(_dbContext);
                }
                return _brokerRepository;
            }
        }

        private AffiliateRepository _affiliateRepository;

        public AffiliateRepository AffiliateRepo
        {
            get
            {
                if (_affiliateRepository == null)
                {
                    _affiliateRepository = new AffiliateRepository(_dbContext);
                }
                return _affiliateRepository;
            }
        }

        private FactorRepository _factorRepository;

        public FactorRepository FactorRepo
        {
            get
            {
                if (_factorRepository == null)
                {
                    _factorRepository = new FactorRepository(_dbContext);
                }
                return _factorRepository;
            }
        }

        private ProductDetailRepository _productDetailRepository;

        public ProductDetailRepository ProductDetailRepo
        {
            get
            {
                if (_productDetailRepository == null)
                {
                    _productDetailRepository = new ProductDetailRepository(_dbContext);
                }
                return _productDetailRepository;
            }
        }

        private AgentRepository _agentRepository;

        public AgentRepository AgentRepo
        {
            get
            {
                if (_agentRepository == null)
                {
                    _agentRepository = new AgentRepository(_dbContext);
                }
                return _agentRepository;
            }
        }

        private SellRepository _sellRepository;

        public SellRepository SellRepo
        {
            get
            {
                if (_sellRepository == null)
                {
                    _sellRepository = new SellRepository(_dbContext);
                }
                return _sellRepository;
            }
        }

        private AffiliateParameterRepository  _affiliateParameter;

        public AffiliateParameterRepository AffiliateParameterRepo
        {
            get
            {
                if (_affiliateParameter == null)
                {
                    _affiliateParameter = new AffiliateParameterRepository(_dbContext);
                }
                return _affiliateParameter;
            }
        }
        private SupportRepository _supportRepository;

        public SupportRepository SupportRepo
        {
            get
            {
                if (_supportRepository == null)
                {
                    _supportRepository = new SupportRepository(_dbContext);
                }
                return _supportRepository;
            }
        }
        private SupportTypeRepository _supportTypeRepository;

        public SupportTypeRepository SupportTypeRepo
        {
            get
            {
                if (_supportTypeRepository == null)
                {
                    _supportTypeRepository = new SupportTypeRepository(_dbContext);
                }
                return _supportTypeRepository;
            }
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }


        #region BackUpFromDb
        public bool BackUpFromDb(string path)
        {
            try
            {
                _dbContext.Database.ExecuteSqlRaw("BACKUP DATABASE BaseProj TO DISK = {0}", path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        
    }
}
