using Microsoft.Extensions.Options;
using ProjectManager.Entity;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface ITeacherService
    {
        Task<SpecializedResponse> GetTeacherBySpecializedAsync(PagingRequest request, string token);
        Task<TeacherResponse> GetAllAsync(TeacherRequest request, string token);
        Task<SaveResponse> SaveAsync(Teacher request, string token);
        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
        Task<TeacherResponse> GetAllTeacherAsync(string token);
    }

    public class TeacherService : ITeacherService
    {
        private readonly AppSettings _appSettings;

        public TeacherService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Teacher_Delete, token);
            return response;
        }

        public async Task<TeacherResponse> GetAllAsync(TeacherRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<TeacherResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Teacher_GetAll, request.DepartmentId, request.SpecializedId, request.Page, request.PageSize, request.SearchText),
                token);

            return response;
        }

        public async Task<TeacherResponse> GetAllTeacherAsync(string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<TeacherResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Teacher_GetAllTeacher),
                token);

            return response;
        }

        public async Task<SpecializedResponse> GetTeacherBySpecializedAsync(PagingRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<SpecializedResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Teacher_GetTeacherBySpecialized, request.Page,request.PageSize, request.SearchText),
                token);

            return response;
        }

        public async Task<SaveResponse> SaveAsync(Teacher request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Teacher_Save, token);
            return response;
        }
    }
}
