using Dapper;
using Microsoft.Extensions.Configuration;
using ProjectManager.Connection;
using ProjectManager.Shared.Common.Configuration;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Helper;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ProjectManager.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }

        DatabaseContext DbContext { get; }

        IDbTransaction Transaction { get; }

        string ConnectionString { get; }

        Task<IDbTransaction> OpenTransaction();

        Task<IDbTransaction> OpenTransaction(IsolationLevel level);

        void CommitTransaction(bool disposeTrans = true);

        void RollbackTransaction(bool disposeTrans = true);

        void BeginTransaction();

        int SaveChanges();

        void Commit();

        void Rollback();

        Task<int> SaveChangesAsync();

        IClasssRepository ClasssRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IProjectListRepository ProjectListRepository { get; }
        IInternRepository InternRepository { get; }

        ISchoolYearRepository SchoolYearRepository { get; }
        ISpecializedRepository SpecializedRepository { get; }
        IStudentRepository StudentRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        ITrainingSystemRepository TrainingSystemRepository { get; }
        IUserManagementRepository UserManagementRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {

        #region Dapper Transaction

        protected readonly IConfiguration Config;
        protected string CnStr = Configuration.Instance.GetConfig<string>(Constants.ConnectionStrings, Constants.MainConnectionString);
        public string ConnectionString
        {
            get => CnStr;
        }
        protected IDbConnection Cn = null;
        public IDbConnection Connection
        {
            get => Cn;
        }

        protected DatabaseContext Context = null;
        public DatabaseContext DbContext
        {
            get => Context;
        }
        protected IDbTransaction Trans = null;
        public IDbTransaction Transaction
        {
            get => Trans;
        }
        public UnitOfWork(IConfiguration config, DatabaseContext context)
        {
            Config = config;
            CnStr = Config.GetConnectionString(ConnectionHelper.DatabaseName.MainConnectionString.ToString());
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            Cn = new SqlConnection(CnStr);
            Context = context;
        }
        /// <summary>
        /// Open a transaction
        /// </summary>
        public async Task<IDbTransaction> OpenTransaction()
        {
            if (Trans != null)
                throw new Exception("A transaction is already open, you need to use a new DbContext for parallel job.");

            if (Cn.State == ConnectionState.Closed)
            {
                if (!(Cn is DbConnection))
                    throw new Exception("Connection object does not support OpenAsync.");

                await (Cn as DbConnection)?.OpenAsync();
            }

            Trans = Cn.BeginTransaction();

            return Trans;
        }


        /// <summary>
        /// Open a transaction with a specified isolation level
        /// </summary>
        public async Task<IDbTransaction> OpenTransaction(IsolationLevel level)
        {
            if (Trans != null)
                throw new Exception("A transaction is already open, you need to use a new DbContext for parallel job.");

            if (Cn.State == ConnectionState.Closed)
            {
                if (!(Cn is DbConnection))
                    throw new Exception("Connection object does not support OpenAsync.");

                await (Cn as DbConnection).OpenAsync();
            }

            Trans = Cn.BeginTransaction(level);

            return Trans;
        }

        /// <summary>
        /// Commit the current transaction, and optionally dispose all resources related to the transaction.
        /// </summary>
        public void CommitTransaction(bool disposeTrans = true)
        {
            if (Trans == null)
                throw new Exception("DB Transaction is not present.");

            Trans.Commit();
            if (disposeTrans) Trans.Dispose();
            if (disposeTrans) Trans = null;
        }

        /// <summary>
        /// Rollback the transaction and all the operations linked to it, and optionally dispose all resources related to the transaction.
        /// </summary>
        public void RollbackTransaction(bool disposeTrans = true)
        {
            if (Trans == null)
                throw new Exception("DB Transaction is not present.");

            Trans.Rollback();
            if (disposeTrans) Trans.Dispose();
            if (disposeTrans) Trans = null;
        }

        /// <summary>
        /// Will be call at the end of the service (ex : transient service in api net core)
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
            Trans?.Dispose();
            Cn?.Close();
            Cn?.Dispose();
        }

        #endregion

        #region Transaction

        public virtual void BeginTransaction() => Context.Database.BeginTransaction();

        public virtual void Commit() => Context.Database.CommitTransaction();

        public virtual void Rollback() => Context.Database.RollbackTransaction();

        public virtual int SaveChanges() => Context.SaveChanges();
        public virtual Task<int> SaveChangesAsync() => Context.SaveChangesAsync();
        #endregion

        private IClasssRepository _classsRepository;
        public IClasssRepository ClasssRepository
        {
            get
            {
                return _classsRepository ??= new ClasssRepository(this);
            }
        }

        private IDepartmentRepository _departmentRepository;
        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                return _departmentRepository ??= new DepartmentRepository(this);
            }
        }

        private IProjectListRepository _projectListRepository;
        public IProjectListRepository ProjectListRepository
        {
            get
            {
                return _projectListRepository ??= new ProjectListRepository(this);
            }
        }

        private ISchoolYearRepository _schoolYearRepository;
        public ISchoolYearRepository SchoolYearRepository
        {
            get
            {
                return _schoolYearRepository ??= new SchoolYearRepository(this);
            }
        }

        private ISpecializedRepository _specializedRepository;
        public ISpecializedRepository SpecializedRepository
        {
            get
            {
                return _specializedRepository ??= new SpecializedRepository(this);
            }
        }

        private IStudentRepository _studentRepository;
        public IStudentRepository StudentRepository
        {
            get
            {
                return _studentRepository ??= new StudentRepository(this);
            }
        }

        private ITeacherRepository _teacherRepository;
        public ITeacherRepository TeacherRepository
        {
            get
            {
                return _teacherRepository ??= new TeacherRepository(this);
            }
        }

        private ITrainingSystemRepository _trainingSystemRepository;
        public ITrainingSystemRepository TrainingSystemRepository
        {
            get
            {
                return _trainingSystemRepository ??= new TrainingSystemRepository(this);
            }
        }

        private IUserManagementRepository _userManagementRepository;
        public IUserManagementRepository UserManagementRepository
        {
            get
            {
                return _userManagementRepository ??= new UserManagementRepository(this);
            }
        }

        public IInternRepository _internRepository;
        public IInternRepository InternRepository
        {
            get
            {
                return _internRepository ??= new InternRepository(this);
            }
        }
    }
}
