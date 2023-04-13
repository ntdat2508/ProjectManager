using Microsoft.Extensions.Options;
using ProjectManager.Entity;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface ISpecializedService
    {
        Task<SpecializedResponse> GetAllAsync(PagingRequest request, string token);
        Task<SaveResponse> SaveAsync(Specialized request, string token);
        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
        Task<SpecializedResponse> GetAllSpecializedAsync(string token);
    }

    public class SpecializedService : ISpecializedService
    {
        private readonly AppSettings _appSettings;

        public SpecializedService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Specialized_Delete, token);
            return response;
        }

        public async Task<SpecializedResponse> GetAllAsync(PagingRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<SpecializedResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Specialized_GetAll, request.Page, request.PageSize, request.SearchText),
                token);

            return response;
        }

        public async Task<SpecializedResponse> GetAllSpecializedAsync(string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<SpecializedResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Specialized_GetAllSpecialized),
                token);

            return response;
        }

        public async Task<SaveResponse> SaveAsync(Specialized request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Specialized_Save, token);
            return response;
        }
    }
}
