using Dapper;
using ProjectManager.Entity;
using ProjectManager.Repository;
using ProjectManager.Shared.Common.Pagging;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.ViewModel;
using Serilog;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProjectManager.Services.Api
{
    public interface IClasssService
    {
        Task<IPagedResults<ClasssViewModel>> GetAllAsync(ClasssRequest request);
        Task<IPagedResult<bool>> SaveAsync(Classs request);
        Task<IPagedResult<bool>> DeleteAsync(long id, string username);
        IPagedResults<Classs> GetAllClasssAsync();
    }

    public class ClasssService : IClasssService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperRepository _dapperRepository;

        public ClasssService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository)
        {
            _unitOfWork = unitOfWork;
            _dapperRepository = dapperRepository;
        }

        public async Task<IPagedResult<bool>> DeleteAsync(long id, string username)
        {
            using (var tran = await _unitOfWork.OpenTransaction())
            {
                try
                {
                    if (id <= 0)
                    {
                        return new PagedResult<bool>
                        {
                            ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                            ResponseMessage = Constants.Message.IdNotFound,
                            Data = false
                        };
                    }

                    var delete = await _unitOfWork.ClasssRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                    if (delete == null || delete.Id <= 0)
                    {
                        return new PagedResult<bool>
                        {
                            ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                            ResponseMessage = Constants.Message.RecordNotFoundMessage,
                            Data = false
                        };
                    }
                    delete.IsDeleted = true;
                    delete.DeletedBy = username;
                    delete.DeletedDate = DateTime.Now;

                    _unitOfWork.ClasssRepository.DeleteWhere(x => x.Id == id && !x.IsDeleted);
                    var result = _unitOfWork.ClasssRepository.Commit();
                    _unitOfWork.CommitTransaction();
                    if (result)
                    {
                        return new PagedResult<bool>
                        {
                            ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                            ResponseMessage = Constants.Message.DeleteSuccess,
                            Data = true
                        };
                    }
                    return new PagedResult<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.InternalServerError),
                        ResponseMessage = Constants.Message.DeleteFail,
                        Data = false
                    };
                }
                catch (Exception ex)
                {
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ClasssService DeleteAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task<IPagedResults<ClasssViewModel>> GetAllAsync(ClasssRequest request)
        {
            try
            {
                var page = request.Page ?? 1;
                var pageSize = request.PageSize ?? 10;

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(Constants.Key, request.SearchText);
                dynamicParameters.Add(Constants.Page, page);
                dynamicParameters.Add(Constants.PageSize, pageSize);
                dynamicParameters.Add(Constants.DepartmentId, request.DepartmentId);
                dynamicParameters.Add(Constants.SpecializedId, request.SpecializedId);
                dynamicParameters.Add(Constants.SchoolYearId, request.SchoolYearId);
                var list = await _dapperRepository.QueryMultipleUsingStoreProcAsync<ClasssViewModel>("uspClasss_SelectAll", dynamicParameters);

                if (null == list)
                {
                    return new PagedResults<ClasssViewModel>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                var listStudent = _unitOfWork.StudentRepository.GetAll();
                var i = 0;
                list.ToList().ForEach(x =>
                {
                    i++;
                    x.STT = (page - 1) * pageSize + i;
                    x.Number = listStudent?.Where(d => d.ClasssId == x.Id).Count() ?? 0;
                });
                return new PagedResults<ClasssViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ClasssService GetAllAsync: " + ex.ToString());
                throw;
            }
        }

        public IPagedResults<Classs> GetAllClasssAsync()
        {
            try
            {
                var list = _unitOfWork.ClasssRepository.GetAll();
                if (null == list)
                {
                    return new PagedResults<Classs>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                return new PagedResults<Classs>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.Count()
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ClasssService GetAllClasssAsync: " + ex.ToString());
                throw;
            }
        }

        public async Task<IPagedResult<bool>> SaveAsync(Classs request)
        {
            using (var tran = await _unitOfWork.OpenTransaction())
            {
                try
                {
                    var result = false;
                    if (request.Id > 0)
                    {
                        var update = await _unitOfWork.ClasssRepository.GetSingleAsync(x => x.Id == request.Id && !x.IsDeleted);
                        if (update == null || update.Id <= 0)
                        {
                            return new PagedResult<bool>
                            {
                                ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                                ResponseMessage = Constants.Message.RecordNotFoundMessage,
                                Data = false
                            };
                        }

                        update.Name = request.Name;
                        update.DepartmentId = request.DepartmentId;
                        update.SpecializedId = request.SpecializedId;
                        update.SchoolYearId = request.SchoolYearId;
                        update.ModifiedBy = request.ModifiedBy;
                        update.ModifiedDate = DateTime.Now;
                        update.ID_Classs = request.ID_Classs;
                        _unitOfWork.ClasssRepository.Update(update);
                        result = _unitOfWork.ClasssRepository.Commit();
                    }
                    else
                    {
                        request.CreatedBy = request.CreatedBy;
                        request.CreatedDate = DateTime.Now;

                        _unitOfWork.ClasssRepository.Add(request);
                        result = _unitOfWork.ClasssRepository.Commit();
                    }
                    _unitOfWork.CommitTransaction();
                    if (result)
                    {
                        return new PagedResult<bool>
                        {
                            ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                            ResponseMessage = Constants.Message.SaveSuccess,
                            Data = true
                        };
                    }
                    return new PagedResult<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.InternalServerError),
                        ResponseMessage = Constants.Message.SaveFail,
                        Data = false
                    };
                }
                catch (Exception ex)
                {
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ClasssService SaveAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
