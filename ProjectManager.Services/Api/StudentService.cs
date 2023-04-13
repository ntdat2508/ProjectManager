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
    public interface IStudentService
    {
        Task<IPagedResults<StudentViewModel>> GetStudentByClasssAsync(long classsId);
        Task<IPagedResults<StudentViewModel>> GetAllAsync(StudentRequest request);
        Task<IPagedResult<bool>> SaveAsync(Student request);
        Task<IPagedResult<bool>> DeleteAsync(long id, string username);
        IPagedResults<Student> GetAllStudentAsync();
        Task<StudentViewModel> GetSelectAllByUsernameAsync(string username);
    }

    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperRepository _dapperRepository;

        public StudentService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository)
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

                    var delete = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.Id == id);
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

                    _unitOfWork.StudentRepository.DeleteWhere(x => x.Id == id);
                    var result = _unitOfWork.StudentRepository.Commit();
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentService DeleteAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task<IPagedResults<StudentViewModel>> GetAllAsync(StudentRequest request)
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
                dynamicParameters.Add(Constants.ClasssId, request.ClasssId);
                var list = await _dapperRepository.QueryMultipleUsingStoreProcAsync<StudentViewModel>("uspStudent_SelectAll", dynamicParameters);

                if (null == list)
                {
                    return new PagedResults<StudentViewModel>
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
                });
                return new PagedResults<StudentViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentService GetAllAsync: " + ex.ToString());
                throw;
            }
        }

        public IPagedResults<Student> GetAllStudentAsync()
        {
            try
            {
                var list = _unitOfWork.StudentRepository.GetAll().Where(x => !x.IsDeleted);
                if (null == list)
                {
                    return new PagedResults<Student>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                return new PagedResults<Student>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = Constants.Message.Successfully,
                    Data = list,
                    TotalRecords = list.Count()
                };
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentService GetAllStudentAsync: " + ex.ToString());
                throw;
            }
        }

        public async Task<StudentViewModel> GetSelectAllByUsernameAsync(string username)
        {
            try
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(Constants.Key, username);
                var list = await _dapperRepository.QueryMultipleUsingStoreProcAsync<StudentViewModel>("uspStudent_SelectAllByUsername", dynamicParameters);

                if (null == list || 0 == list.Count())
                {
                    return new StudentViewModel();
                }

                return list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentService GetSelectAllByUsernameAsync: " + ex.ToString());
                throw;
            }
        }

        public async Task<IPagedResults<StudentViewModel>> GetStudentByClasssAsync(long classsId)
        {
            try
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(Constants.Key, classsId);
                var list = await _dapperRepository.QueryMultipleUsingStoreProcAsync<StudentViewModel>("uspStudent_SelectByClasss", dynamicParameters);

                if (null == list)
                {
                    return new PagedResults<StudentViewModel>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                        ResponseMessage = Constants.Message.RecordNotFoundMessage,
                        Data = null,
                        TotalRecords = 0
                    };
                }

                return new PagedResults<StudentViewModel>
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

        public async Task<IPagedResult<bool>> SaveAsync(Student request)
        {
            using (var tran = await _unitOfWork.OpenTransaction())
            {
                try
                {
                    var result = false;
                    if (request.Id > 0)
                    {
                        var checkDuplicate = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.Username == request.Username && x.Id != request.Id && !x.IsDeleted);
                        if (checkDuplicate != null)
                        {
                            return new PagedResult<bool>
                            {
                                ResponseCode = Convert.ToInt32(HttpStatusCode.Conflict),
                                ResponseMessage = Constants.Message.Duplicate,
                                Data = false
                            };
                        }

                        var update = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.Id == request.Id && !x.IsDeleted);
                        if (update == null || update.Id <= 0)
                        {
                            return new PagedResult<bool>
                            {
                                ResponseCode = Convert.ToInt32(HttpStatusCode.NotFound),
                                ResponseMessage = Constants.Message.RecordNotFoundMessage,
                                Data = false
                            };
                        }
                        update.ID_Student = request.ID_Student;
                        update.Name = request.Name;
                        update.PhoneNumber = request.PhoneNumber;
                        update.Email = request.Email;
                        update.Address = request.Address;
                        update.Gender = request.Gender;
                        update.DateOfBirth = request.DateOfBirth;
                        update.SpecializedId = request.SpecializedId;
                        update.TrainingSystemId = request.TrainingSystemId;
                        update.DepartmentId = request.DepartmentId;
                        update.ClasssId = request.ClasssId;
                        update.ModifiedBy = request.ModifiedBy;
                        update.ModifiedDate = DateTime.Now;

                        _unitOfWork.StudentRepository.Update(update);
                        result = _unitOfWork.StudentRepository.Commit();
                    }
                    else
                    {
                        var checkDuplicate = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.Username == request.Username);
                        if (checkDuplicate != null)
                        {
                            return new PagedResult<bool>
                            {
                                ResponseCode = Convert.ToInt32(HttpStatusCode.Conflict),
                                ResponseMessage = Constants.Message.Duplicate,
                                Data = false
                            };
                        }

                        request.Username = DateTime.Now.Year + MurmurHashHelper.Hash(Guid.NewGuid().ToString() + DateTime.Now).ToString();
                        request.Password = EncriptFunctionsHelper.GenerateMd5String(request.Username);
                        request.CreatedBy = request.CreatedBy;
                        request.CreatedDate = DateTime.Now;

                        _unitOfWork.StudentRepository.Add(request);
                        result = _unitOfWork.StudentRepository.Commit();
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
                    Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentService SaveAsync: " + ex.ToString());
                    _unitOfWork.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
