using Dapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectManager.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectManager.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllUsingDapper(string storedProcedure);
        Task<T> GetByIdUsingDapper(long id, string storedProcedure);
        Task<bool> InsertUsingDapper(string storedProcedure, object parms);
        Task<bool> BulkInsertUsingDapper(string storedProcedure, object parms);
        Task<bool> UpdateUsingDapper(T obj, string storedProcedure, object parms);
        Task<bool> DeleteUsingDapper(long id, string storedProcedure);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllNoneDeleted();
        void AddRange(List<T> entity);
        int Count();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);
        T GetSingleNoneDeleted(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        bool Commit();
        Task<bool> CommitAsync();
        void BulkInsert(IList<T> items);
        void BulkUpdate(IList<T> items);
        void BulkDelete(IList<T> items);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DatabaseContext _context;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _context = unitOfWork.DbContext;
        }

        public void Add(T entity)
        {
            _context.Entry(entity);
            _context.Set<T>().Add(entity);
        }

        public void AddRange(List<T> entity)
        {
            _context.Set<T>().AddRange(entity);
        }

        public virtual void BulkDelete(IList<T> items) => _context.BulkDelete(items);

        public virtual void BulkInsert(IList<T> items) => _context.BulkInsert(items);

        public async Task<bool> BulkInsertUsingDapper(string storedProcedure, object parms)
        {
            await _unitOfWork.Connection.ExecuteAsync
                 (storedProcedure, parms, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);

            return true;
        }

        public virtual void BulkUpdate(IList<T> items) => _context.BulkUpdate(items);

        public bool Commit()
        {
            var recordsCommittedCount = _context.SaveChanges();
            return (recordsCommittedCount > 0);
        }

        public async Task<bool> CommitAsync()
        {
            var recordsCommittedCount = await _context.SaveChangesAsync();
            return (recordsCommittedCount > 0);
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public void Delete(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public async Task<bool> DeleteUsingDapper(long id, string storedProcedure)
        {
            var p = new DynamicParameters();
            p.Add("@Id", id);

            await _unitOfWork.Connection.ExecuteAsync
                (storedProcedure, p, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);

            return true;
        }

        public void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> query = _context.Set<T>();
            return query.AsQueryable();
        }

        public IQueryable<T> GetAllNoneDeleted()
        {
            IQueryable<T> query = _context.Set<T>();
            return query.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllUsingDapper(string storedProcedure)
        {
            var entities = await _unitOfWork.Connection.QueryAsync<T>
                 (storedProcedure, commandType: CommandType.StoredProcedure);

            return entities;
        }

        public async Task<T> GetByIdUsingDapper(long id, string storedProcedure)
        {
            var p = new DynamicParameters();
            p.Add("@Id", id);

            var entity = await _unitOfWork.Connection.QuerySingleOrDefaultAsync<T>
                (storedProcedure, p, commandType: CommandType.StoredProcedure);

            return entity;
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).FirstOrDefault();
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public T GetSingleNoneDeleted(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public async Task<T> GetSingleNoneDeletedAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> InsertUsingDapper(string storedProcedure, object parms)
        {
            await _unitOfWork.Connection.ExecuteAsync
             (storedProcedure, parms, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }

        public void Update(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public async Task<bool> UpdateUsingDapper(T obj, string storedProcedure, object parms)
        {
            await _unitOfWork.Connection.ExecuteAsync
              (storedProcedure, parms, commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);

            return true;
        }
    }
}
