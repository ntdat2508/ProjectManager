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
    public interface IInternService
    {
        Task<InternResponse> GetAllInternAsync(string token);
        Task<InternResponse> GetAllAsync(InternRequest request, string token);
        Task<SaveResponse> SaveAsync(Intern request, string token);
        Task<SaveResponse> MarkAsync(Intern request, string token);

        Task<SaveResponse> DeleteAsync(DeleteRequest request, string token);
        List<StatusViewModel> GetAllListStatusAsync();

    }
    public class InterService : IInternService
    {
        private readonly AppSettings _appSettings;
        public InterService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<SaveResponse> DeleteAsync(DeleteRequest request, string token)
        {
        var client = new HttpClientHelper();
        var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Intern_Delete, token);
        return response;
    }

        public async Task<InternResponse> GetAllAsync(InternRequest request, string token)
        {
        var client = new HttpClientHelper();
        var response = await client.GetAsync<InternResponse>(_appSettings.BaseUri,
            string.Format(_appSettings.Intern_GetAll,  request.TeacherId, request.StudentId, request.Page, request.PageSize, request.SearchText, request.Status),
            token);

        return response;
    }

        public async Task<InternResponse> GetAllInternAsync(string token)
        {
               var client = new HttpClientHelper();
            var response = await client.GetAsync<InternResponse>(_appSettings.BaseUri,
                string.Format(_appSettings.Intern_GetAllIntern),
                token);

            return response;        }

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

        public async Task<SaveResponse> MarkAsync(Intern request, string token)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Intern_Mark, token);
            return response;
        }

        public async Task<SaveResponse> SaveAsync(Intern request, string token)
        {
        var client = new HttpClientHelper();
        var response = await client.PostAsync<SaveResponse>(request, _appSettings.BaseUri, _appSettings.Intern_Save, token);
        return response;
    }


    }
}
