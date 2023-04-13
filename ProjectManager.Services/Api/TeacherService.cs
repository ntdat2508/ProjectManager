using Dapper;
using ProjectManager.Entity;
using ProjectManager.Repository;
using ProjectManager.Shared.Common.Pagging;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Helper;
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
    public interface ITeacherService
    {
        Task<IPagedResults<SpecializedViewModel>> GetTeacherBySpecializedAsync(PagingRequest request);
        Task<IPagedResults<TeacherViewModel>> GetAllAsync(TeacherRequest request);
        Task<IPagedResult<bool>> SaveAsync(Teacher request);
        Task<IPagedResult<bool>> DeleteAsync(long id, string username);
        IPagedResults<Teacher> GetAllTeacherAsync();
    }

    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperRepository _dapperRepository;

        public TeacherService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository)
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

                    var delete = await _unitOfWork.TeacherRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
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

                    _unitOfWork.TeacherRepository.DeleteWhere(x => x.Id == id && !x.IsDeleted);
                    var result = _unitOfWork.TeacherRepository.Commit();
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " TeacherService DeleteAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task<IPagedResults<TeacherViewModel>> GetAllAsync(TeacherRequest request)
        {
            try
            {
                var page = request.Page ?? 1;
                var pageSize = request.PageSize ?? 10;

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(Constants.Key, request.SearchText);
                dynamicParameters.Add(Constants.DepartmentId, request.DepartmentId);
                dynamicParameters.Add(Constants.SpecializedId, request.SpecializedId);
                dynamicParameters.Add(Constants.Page, page);
                dynamicParameters.Add(Constants.PageSize, pageSize);
                var list = await _dapperRepository.QueryMultipleUsingStoreProcAsync<TeacherViewModel>("uspTeacher_SelectAll", dynamicParameters);

                if (null == list)
                {
                    return new PagedResults<TeacherViewModel>
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

                return new PagedResults<TeacherViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " TeacherService GetAllAsync: " + ex.ToString());
                throw;
            }
        }

        public async Task<IPagedResults<SpecializedViewModel>> GetTeacherBySpecializedAsync(PagingRequest request)
        {
            try
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(Constants.Key, request.SearchText);
                dynamicParameters.Add(Constants.Page, request.Page);
                dynamicParameters.Add(Constants.PageSize, request.PageSize);
                var list = await _dapperRepository.QueryMultipleUsingStoreProcAsync<SpecializedViewModel>("uspTeacher_SelectBySpecialized", dynamicParameters);

                if (null == list)
                {
                    return new PagedResults<SpecializedViewModel>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                return new PagedResults<SpecializedViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentService GetStudentByClasssAsync: " + ex.ToString());
                throw;
            }
        }

        public IPagedResults<Teacher> GetAllTeacherAsync()
        {
            try
            {
                var list = _unitOfWork.TeacherRepository.GetAll();
                if (null == list)
                {
                    return new PagedResults<Teacher>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                return new PagedResults<Teacher>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.Count()
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " TeacherService GetAllTeacherAsync: " + ex.ToString());
                throw;
            }
        }

        public async Task<IPagedResult<bool>> SaveAsync(Teacher request)
        {
            using (var tran = await _unitOfWork.OpenTransaction())
            {
                try
                {
                    var result = false;
                    if (request.Id > 0)
                    {
                        var update = await _unitOfWork.TeacherRepository.GetSingleAsync(x => x.Id == request.Id && !x.IsDeleted);
                        if (update == null || update.Id <= 0)
                        {
                            return new PagedResult<bool>
                            {
                                ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                                ResponseMessage = Constants.Message.RecordNotFoundMessage,
                                Data = false
                            };
                        }
                        update.ID_Teacher = request.ID_Teacher;
                        update.Name = request.Name;
                        update.PhoneNumber = request.PhoneNumber;
                        update.Email = request.Email;
                        update.Address = request.Address;
                        update.DateOfBirth = request.DateOfBirth;
                        update.Gender = request.Gender;
                        update.DepartmentId = request.DepartmentId;
                        update.SpecializedId = request.SpecializedId;
                        update.ModifiedBy = request.ModifiedBy;
                        update.ModifiedDate = DateTime.Now;

                        _unitOfWork.TeacherRepository.Update(update);
                        result = _unitOfWork.TeacherRepository.Commit();
                    }
                    else
                    {
                        request.Username = "MGV" + MurmurHashHelper.Hash(Guid.NewGuid().ToString());
                        request.Password = EncriptFunctionsHelper.GenerateMd5String(request.Username);
                        request.CreatedBy = request.CreatedBy;
                        request.CreatedDate = DateTime.Now;

                        _unitOfWork.TeacherRepository.Add(request);
                        result = _unitOfWork.TeacherRepository.Commit();
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " TeacherService SaveAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
