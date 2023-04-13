using Microsoft.Extensions.Options;
using ProjectManager.Entity;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface IDepartmentService
    {
        Task<DepartmentResponse> GetAllDepartmentAsync(string token);
        Task<DepartmentResponse> GetAllAsync(PagingRequest request, string token);
        Task<SaveResponse> SaveAsync(Department request, string token);
        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly AppSettings _appSettings;

        public DepartmentService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<DepartmentResponse> GetAllDepartmentAsync(string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<DepartmentResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Department_GetAllDepartment),
                token);

            return response;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Department_Delete, token);
            return response;
        }

        public async Task<DepartmentResponse> GetAllAsync(PagingRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<DepartmentResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Department_GetAll, request.Page, request.PageSize, request.SearchText),
                token);

            return response;
        }

        public async Task<SaveResponse> SaveAsync(Department request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Department_Save, token);
            return response;
        }
    }
}
