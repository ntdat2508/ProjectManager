using Microsoft.Extensions.Options;
using ProjectManager.Entity;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using ProjectManager.Shared.Model.ViewModel;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface IStudentService
    {
        Task<StudentResponse> GetStudentByClasssAsync(long classsId, string token);
        Task<StudentResponse> GetAllAsync(StudentRequest request, string token);
        Task<SaveResponse> SaveAsync(Student request, string token);
        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
        Task<StudentResponse> GetAllStudentAsync(string token);
        Task<StudentViewModel> GetSelectAllByUsernameAsync(string userName, string token);


    }

    public class StudentService : IStudentService
    {
        private readonly AppSettings _appSettings;

        public StudentService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Student_Delete, token);
            return response;
        }

        public async Task<StudentResponse> GetAllAsync(StudentRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<StudentResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Student_GetAll, request.DepartmentId, request.SpecializedId, request.ClasssId, request.Page, request.PageSize, request.SearchText),
                token);

            return response;
        }

        public async Task<StudentResponse> GetAllStudentAsync(string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<StudentResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Student_GetAllStudent),
                token);

            return response;
        }

        public async Task<StudentResponse> GetStudentByClasssAsync(long classsId, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<StudentResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Student_GetStudentByClasss, classsId),
                token);

            return response;
        }

        public async Task<SaveResponse> SaveAsync(Student request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Student_Save, token);
            return response;
        }

        public async Task<StudentViewModel> GetSelectAllByUsernameAsync(string userName, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<StudentViewModel>(_appSettings.BaseUri,
                string.Format(_appSettings.Student_GetSelectAllByUsername, userName),
                token);

            return response;
        }
    }
}
