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
    public interface IInternService
    {
        Task<IPagedResults<InternViewModel>> GetAllAsync(InternRequest request);
        Task<IPagedResult<bool>> SaveAsync(Intern request);
        Task<IPagedResult<bool>> MarkAsync(Intern request);
        Task<IPagedResult<bool>> DeleteAsync(long id, string username);
        IPagedResults<Intern> GetAllInternAsync();
    }

    public class InternService : IInternService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperRepository _dapperRepository;

        public InternService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository)
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

                    var delete = await _unitOfWork.InternRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);

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

                    _unitOfWork.InternRepository.DeleteWhere(x => x.Id == id && !x.IsDeleted);
                    var result = _unitOfWork.InternRepository.Commit();
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " InternService DeleteAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task<IPagedResults<InternViewModel>> GetAllAsync(InternRequest request)
        {
            try
            {
                var page = request.Page ?? 1;
                var pageSize = request.PageSize ?? 10;

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(Constants.Key, request.SearchText);
                dynamicParameters.Add(Constants.Page, page);
                dynamicParameters.Add(Constants.PageSize, pageSize);
                dynamicParameters.Add(Constants.TeacherId, request.TeacherId);
                dynamicParameters.Add(Constants.SchoolYearId, request.SchoolYearId);
                dynamicParameters.Add(Constants.Status, request.Status);
                var list = await _dapperRepository.QueryMultipleUsingStoreProcAsync<InternViewModel>("uspIntern_SelectAll", dynamicParameters);

                if (null == list)
                {

                    return new PagedResults<InternViewModel>
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
                return new PagedResults<InternViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " InternService GetAllAsync: " + ex.ToString());
                throw;
            }
        }

        public IPagedResults<Intern> GetAllInternAsync()
        {
            try
            {
                var list = _unitOfWork.InternRepository.GetAll();
                if (null == list)
                {
                    return new PagedResults<Intern>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                return new PagedResults<Intern>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.Count()
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " InternService GetAllInternAsync: " + ex.ToString());
                throw;
            }
        }

        public async Task<IPagedResult<bool>> MarkAsync(Intern request)
        {
            using (var tran = await _unitOfWork.OpenTransaction())
            {
                try
                {
                    var result = false;
                    if (request.Id > 0)
                    {
                        var update = await _unitOfWork.InternRepository.GetSingleAsync(x => x.Id == request.Id && !x.IsDeleted);
                        if (update == null || update.Id <= 0)
                        {
                            return new PagedResult<bool>
                            {
                                ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                                ResponseMessage = Constants.Message.RecordNotFoundMessage,
                                Data = false
                            };
                        }

                        update.Point = request.Point;
                        update.ModifiedBy = request.ModifiedBy;
                        update.ModifiedDate = DateTime.Now;

                        _unitOfWork.InternRepository.Update(update);
                        result = _unitOfWork.InternRepository.Commit();
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " InternService MarkAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task<IPagedResult<bool>> SaveAsync(Intern request)
        {
            using (var tran = await _unitOfWork.OpenTransaction())
            {
                try
                {
                    var result = false;
                    if (request.Id > 0)
                    {
                        var update = await _unitOfWork.InternRepository.GetSingleAsync(x => x.Id == request.Id && !x.IsDeleted);
                        if (update == null || update.Id <= 0)
                        {
                            return new PagedResult<bool>
                            {
                                ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                                ResponseMessage = Constants.Message.RecordNotFoundMessage,
                                Data = false
                            };
                        }
                        update.ID_Intern = request.ID_Intern;
                        update.Name = request.Name;
                        update.TeacherId = request.TeacherId;
                        update.Point = request.Point;
                        update.StudentId = request.StudentId;
                        update.LinkDownload = request.LinkDownload;


                        _unitOfWork.InternRepository.Update(update);
                        result = _unitOfWork.ClasssRepository.Commit();
                    }
                    else
                    {
                        request.CreatedBy = request.CreatedBy;
                        request.CreatedDate = DateTime.Now;

                        _unitOfWork.InternRepository.Add(request);
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " InternService SaveAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
        
