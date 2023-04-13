using Microsoft.Extensions.Options;
using ProjectManager.Entity;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface ISchoolYearService
    {
        Task<SchoolYearResponse> GetAllAsync(PagingRequest request, string token);
        Task<SaveResponse> SaveAsync(SchoolYear request, string token);
        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
        Task<SchoolYearResponse> GetAllSchoolYearAsync(string token);
    }

    public class SchoolYearService : ISchoolYearService
    {
        private readonly AppSettings _appSettings;

        public SchoolYearService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.SchoolYear_Delete, token);
            return response;
        }

        public async Task<SchoolYearResponse> GetAllAsync(PagingRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<SchoolYearResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.SchoolYear_GetAll, request.Page, request.PageSize, request.SearchText),
                token);

            return response;
        }

        public async Task<SchoolYearResponse> GetAllSchoolYearAsync(string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<SchoolYearResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.SchoolYear_GetAllSchoolYear),
                token);

            return response;
        }

        public async Task<SaveResponse> SaveAsync(SchoolYear request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.SchoolYear_Save, token);
            return response;
        }
    }
}
