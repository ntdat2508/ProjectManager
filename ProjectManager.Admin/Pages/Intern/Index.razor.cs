using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ProjectManager.Admin;
using ProjectManager.Admin.Shared;
using ProjectManager.Admin.Shared.Template;
using ProjectManager.Shared;
using ProjectManager.Entity;
using BlazorInputFile;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Common.Enum;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Intern
{
    
        public class IndexBase : CommonComponentBase
        {
            public InternRequest request { get; set; } = new InternRequest();
            public RadzenDataGrid<InternViewModel> grid;
            public IEnumerable<InternViewModel> data;
            public IEnumerable<Entity.Teacher> listTeacher { get; set; }
        public IEnumerable<Entity.Student> listStudent { get; set; }

        public IEnumerable<Entity.SchoolYear> listSchoolYear { get; set; }

            public List<StatusViewModel> listStatus { get; set; }

            public bool isLoading;
            public int count;
            public int pageSize = 10;
            public int? page = 1;
            
            protected override async Task OnInitializedAsync()
            {
                Logout();
                isLoading = true;
                request.Status = (long)StatusEnum.All;
                var teacher = await _teacherService.GetAllTeacherAsync(token);
                listTeacher = teacher.Data;
                var student = await _studentService.GetAllStudentAsync(token);
                listStudent = student.Data;

                listStatus = _internService.GetAllListStatusAsync();
            }

            public async Task LoadData(LoadDataArgs args)
            {
                isLoading = true;
                page = (args.Skip + pageSize) / pageSize;
                request.PageSize = pageSize;
                request.Page = page;
                if (internStatus > 0)
                {
                    request.Status = internStatus;
                }

                var result = await _internService.GetAllAsync(request, token);
                var message = new NotificationMessage();
                if (result.ResponseCode == 200)
                {
                    data = result.Data;
                
                    count = result.TotalRecords;
                    internStatus = -3;
                }
                else
                {
                    message.Severity = NotificationSeverity.Error;
                    message.Summary = Constants.Message.Fail;
                    message.Detail = result.ResponseMessage;
                    message.Duration = 4000;
                    _notificationService.Notify(message);
                }

                await Delay();
                isLoading = false;
            }

            public async Task OnSearch()
            {
            var result = await _internService.GetAllAsync(request, token);
            var message = new NotificationMessage();

            if (result.ResponseCode == 200)
            {
                if(result.TotalRecords==0)
                {
                    message.Severity = NotificationSeverity.Error;
                    message.Summary = Constants.Message.Fail;
                    message.Detail = Constants.Message.RecordNotFoundMessage;
                    message.Duration = 4000;
                    _notificationService.Notify(message);
                }    
                data = result.Data; // Gán dữ liệu vào bảng
                await grid.Reload(); // Reload bảng để hiển thị dữ liệu mới
            }
        }
        public async Task ShowModalEditIntern(InternViewModel data)
        {
            await _dialogService.OpenAsync<EditInternModal>(data.Id > 0 ? "Sửa dữ liệu" : "Tạo dữ liệu",
            new Dictionary<string, object>() { { "internViewModel", data }, { "grid", grid }, { "listStudent", listStudent },{"listTeacher",listTeacher } });
        }
          public async Task ShowModalDelete(long id)
        {
            await _dialogService.OpenAsync<TemplateConfirm>(Constants.Notify,
            new Dictionary<string, object>() { { "table", Constants.FromDelete.Intern }, { "id", id }, { "gridIntern", grid } });
        }

        public async Task ShowModal(InternViewModel data)
        {
            await _dialogService.OpenAsync<InternModal>("Chi tiết thông tin thực tập",
            new Dictionary<string, object>() { { "internViewModel", data } },
            new DialogOptions() { Width = "700px" });
        }
       
    }
    }
