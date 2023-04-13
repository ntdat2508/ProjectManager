using Microsoft.Extensions.Options;
using ProjectManager.Entity;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface IClasssService
    {
        Task<ClasssResponse> GetAllAsync(ClasssRequest request, string token);
        Task<SaveResponse> SaveAsync(Classs request, string token);
        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
        Task<ClasssResponse> GetAllClasssAsync(string token);
    }

    public class ClasssService : IClasssService
    {
        private readonly AppSettings _appSettings;

        public ClasssService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Classs_Delete, token);
            return response;
        }

        public async Task<ClasssResponse> GetAllAsync(ClasssRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<ClasssResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Classs_GetAll, request.DepartmentId, request.SpecializedId, request.SchoolYearId, request.Page, request.PageSize, request.SearchText),
                token);

            return response;
        }

        public async Task<ClasssResponse> GetAllClasssAsync(string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<ClasssResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Classs_GetAllClasss),
                token);

            return response;
        }

        public async Task<SaveResponse> SaveAsync(Classs request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Classs_Save, token);
            return response;
        }
    }
}
