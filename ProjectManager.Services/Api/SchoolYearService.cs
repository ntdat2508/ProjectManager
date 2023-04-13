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
    public interface ISchoolYearService
    {
        Task<IPagedResults<SchoolYearViewModel>> GetAllAsync(PagingRequest request);
        Task<IPagedResult<bool>> SaveAsync(SchoolYear request);
        Task<IPagedResult<bool>> DeleteAsync(long id, string username);
        IPagedResults<SchoolYear> GetAllSchoolYearAsync();
    }

    public class SchoolYearService : ISchoolYearService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperRepository _dapperRepository;

        public SchoolYearService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository)
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

                    var delete = await _unitOfWork.SchoolYearRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
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

                    _unitOfWork.SchoolYearRepository.Update(delete);
                    var result = _unitOfWork.SchoolYearRepository.Commit();
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SchoolYearService DeleteAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task<IPagedResults<SchoolYearViewModel>> GetAllAsync(PagingRequest request)
        {
            try
            {
                var page = request.Page ?? 1;
                var pageSize = request.PageSize ?? 10;

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(Constants.Key, request.SearchText);
                dynamicParameters.Add(Constants.Page, page);
                dynamicParameters.Add(Constants.PageSize, pageSize);
                var list = await _dapperRepository.QueryMultipleUsingStoreProcAsync<SchoolYearViewModel>("uspSchoolYear_SelectAll", dynamicParameters);

                if (null == list)
                {
                    return new PagedResults<SchoolYearViewModel>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                var i = 0;
                list.ToList().ForEach(x =>
                {
                    i++;
                    x.STT = (page - 1) * pageSize + i;
                });

                return new PagedResults<SchoolYearViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SchoolYearService GetAllAsync: " + ex.ToString());
                throw;
            }
        }

        public IPagedResults<SchoolYear> GetAllSchoolYearAsync()
        {
            try
            {
                var list = _unitOfWork.SchoolYearRepository.GetAll().Where(x => !x.IsDeleted);
                if (null == list)
                {
                    return new PagedResults<SchoolYear>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                return new PagedResults<SchoolYear>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.Count()
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SchoolYearService GetAllSchoolYearAsync: " + ex.ToString());
                throw;
            }
        }

        public async Task<IPagedResult<bool>> SaveAsync(SchoolYear request)
        {
            using (var tran = await _unitOfWork.OpenTransaction())
            {
                try
                {
                    var result = false;
                    if (request.Id > 0)
                    {
                        var update = await _unitOfWork.SchoolYearRepository.GetSingleAsync(x => x.Id == request.Id && !x.IsDeleted);
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
                        update.ModifiedBy = request.ModifiedBy;
                        update.ModifiedDate = DateTime.Now;

                        _unitOfWork.SchoolYearRepository.Update(update);
                        result = _unitOfWork.SchoolYearRepository.Commit();
                    }
                    else
                    {
                        request.CreatedBy = request.CreatedBy;
                        request.CreatedDate = DateTime.Now;

                        _unitOfWork.SchoolYearRepository.Add(request);
                        result = _unitOfWork.SchoolYearRepository.Commit();
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SchoolYearService SaveAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
