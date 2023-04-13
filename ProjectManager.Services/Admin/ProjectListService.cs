using Microsoft.Extensions.Options;
using ProjectManager.Entity;
using ProjectManager.Shared.Common.Enum;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.Response;
using ProjectManager.Shared.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface IProjectListService
    {
        Task<ProjectListResponse> GetAllAsync(ProjectListRequest request, string token);
        Task<SaveResponse> SaveAsync(ProjectList request, string token);
        Task<SaveResponse> MarkAsync(ProjectList request, string token);
        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
        Task<ProjectListResponse> GetAllProjectListAsync(string token);
        List<StatusViewModel> GetAllListStatusAsync();
    }

    public class ProjectListService : IProjectListService
    {
        private readonly AppSettings _appSettings;

        public ProjectListService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.ProjectList_Delete, token);
            return response;
        }

        public async Task<ProjectListResponse> GetAllAsync(ProjectListRequest request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<ProjectListResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.ProjectList_GetAll, request.TeacherId, request.SchoolYearId, request.Page, request.PageSize, request.SearchText, request.Status),
                token);

            return response;
        }

        public List<StatusViewModel> GetAllListStatusAsync()
        {
            var data = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().OrderBy(x => x).ToList();
            var listData = new List<StatusViewModel>();
            foreach (var item in data)
            {
                var model = new StatusViewModel
                {
                    Id = Convert.ToInt64(item),
                    Name = item.GetDescription()
                };

                listData.Add(model);
            }
            return listData;
        }

        public async Task<ProjectListResponse> GetAllProjectListAsync(string token)
        {
            var client = new HttpClientHelper();
            var response = await client.GetAsync<ProjectListResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.ProjectList_GetAllProjectList),
                token);

            return response;
        }

        public async Task<SaveResponse> MarkAsync(ProjectList request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.ProjectList_Mark, token);
            return response;
        }

        public async Task<SaveResponse> SaveAsync(ProjectList request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.ProjectList_Save, token);
            return response;
        }
    }
}
