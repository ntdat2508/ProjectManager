using Microsoft.Extensions.Options;
using ProjectManager.Entity;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface ITrainingSystemService
    {
        Task<TrainingSystemResponse> GetAllTrainingSystemAsync(string token);
        Task<TrainingSystemResponse> GetAllAsync(PagingRequest request, string token);
        Task<SaveResponse> SaveAsync(TrainingSystem request, string token);
        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
    }

    public class TrainingSystemService : ITrainingSystemService
    {
        private readonly AppSettings _appSettings;

        public TrainingSystemService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<TrainingSystemResponse> GetAllTrainingSystemAsync(string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<TrainingSystemResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.TrainingSystem_GetAllTrainingSystem),
                token);

            return response;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.TrainingSystem_Delete, token);
            return response;
        }

        public async Task<TrainingSystemResponse> GetAllAsync(PagingRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<TrainingSystemResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.TrainingSystem_GetAll, request.Page, request.PageSize, request.SearchText),
                token);

            return response;
        }

        public async Task<SaveResponse> SaveAsync(TrainingSystem request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.TrainingSystem_Save, token);
            return response;
        }
    }
}
