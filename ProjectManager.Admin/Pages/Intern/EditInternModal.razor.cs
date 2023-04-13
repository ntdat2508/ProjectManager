using System;
using System.Linq;
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
using ProjectManager.Shared.Common.Enum;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Entity;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProjectManager.Admin.Pages.Intern
{

    public class InternModalEditBase : CommonComponentBase
    {
        [Parameter]
        public RadzenDataGrid<InternViewModel> grid { get; set; }

        [Parameter]
        public InternViewModel internViewModel { get; set; }

        [Parameter]
        public IEnumerable<Entity.Student> listStudent { get; set; }

        [Parameter]
        public IEnumerable<Entity.Teacher> listTeacher { get; set; }

        [Parameter]
        public IEnumerable<Entity.Specialized> listSpecialized { get; set; }

        public Entity.Intern editModel { get; set; } = new Entity.Intern();
        public bool isLoading;
        public bool isShow;
        protected override async Task OnInitializedAsync()
        {

            var teacher = await _teacherService.GetAllTeacherAsync(token);
            listTeacher = teacher.Data;

            var student = await _studentService.GetAllStudentAsync(token);
            listStudent = student.Data;
            isLoading = true;
            if (internViewModel.Id > 0)
            {


                editModel.Id = internViewModel.Id;

                editModel.Name = internViewModel.Name;

                editModel.StudentId = internViewModel.StudentId;

                editModel.TeacherId = internViewModel.TeacherId;

                editModel.LinkDownload = internViewModel.LinkDownload;

                editModel.Point = internViewModel.Point;

                editModel.CreatedBy = internViewModel.CreatedBy;

                editModel.CreatedDate = internViewModel.CreatedDate;

                editModel.ModifiedBy = internViewModel.ModifiedBy;

                editModel.ModifiedDate = internViewModel.ModifiedDate;

                editModel.ID_Intern = internViewModel.ID_Intern;


                isShow = true;
            }
            else
            {
                isShow = false;
            }

            await Delay();
            isLoading = false;
        }

        public void Cancel()
        {
            _dialogService.Close(true);
        }

        public void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
        }

        public async Task OnSubmit()
        {
            var isNum = new Regex("^(?:[1-9]|10|\\d\\.\\d)$");
            var isId = new Regex("^[a-zA-Z0-9]");
            var isName = new Regex("^[a-zA-Z0-9\\p{L}\\s]*$");
            var isValid_name = isName.IsMatch(editModel.Name);
            var isValid_id = isId.IsMatch(editModel.ID_Intern);
            var isPointcheck = isNum.IsMatch(editModel.Point);
            var message = new NotificationMessage();
            message.Duration = 4000;
            editModel.CreatedBy = userName;
            if (isValid_id && isValid_name && isPointcheck)
            {
                if (editModel.Id > 0)
                {
                    editModel.ModifiedBy = userName;
                }
                try
                {
                    var result = await _internService.SaveAsync(editModel, token);


                    await grid.Reload();
                    if (result.ResponseCode == 200 && result.Data == true)
                    {
                        Cancel();
                        message.Severity = NotificationSeverity.Success;
                        message.Summary = Constants.Message.Successfully;
                        await grid.Reload();
                    }
                    else
                    {
                        message.Severity = NotificationSeverity.Error;
                        message.Summary = Constants.Message.Fail;
                    }

                    message.Detail = result.ResponseMessage;
                }
                catch (Exception)
                {

                    message.Severity = NotificationSeverity.Error;
                    message.Summary = Constants.Message.Fail;
                    message.Detail = Constants.Message.Idexist;
                    await grid.Reload();


                }
            }
            else
            {
                message.Severity = NotificationSeverity.Error;
                message.Summary = Constants.Message.Fail;
                message.Detail = Constants.Message.Validation;
                await grid.Reload();
            }



            _notificationService.Notify(message);

        }

    }
    }
