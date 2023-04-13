using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Classs
{
    public class ClasssModalBase : CommonComponentBase
    {
        [Parameter] public RadzenDataGrid<ClasssViewModel> grid { get; set; }
        [Parameter] public ClasssViewModel classsViewModel { get; set; }
        [Parameter] public IEnumerable<Entity.Department> listDepartment { get; set; }
        [Parameter] public IEnumerable<Entity.SchoolYear> listSchoolYear { get; set; }
        [Parameter] public IEnumerable<Entity.Specialized> listSpecialized { get; set; }
        public Entity.Classs editModel { get; set; } = new Entity.Classs();
        public bool isLoading;
        public bool isShow;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            if (classsViewModel.Id > 0)
            {
                editModel.ID_Classs = classsViewModel.ID_Classs;
                editModel.Id = classsViewModel.Id;
                editModel.Name = classsViewModel.Name;
                editModel.DepartmentId = classsViewModel.DepartmentId;
                editModel.SpecializedId = classsViewModel.SpecializedId;
                editModel.SchoolYearId = classsViewModel.SchoolYearId;
                editModel.CreatedBy = classsViewModel.CreatedBy;
                editModel.CreatedDate = classsViewModel.CreatedDate;
                editModel.ModifiedBy = classsViewModel.ModifiedBy;
                editModel.ModifiedDate = classsViewModel.ModifiedDate;
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
            var isId = new Regex("^[a-zA-Z0-9]");
            var isName = new Regex("^[a-zA-Z0-9\\p{L}\\s]*$");


            var isValid_id = isId.IsMatch(editModel.ID_Classs);
            var isValid_name = isName.IsMatch(editModel.Name);

            var message = new NotificationMessage();
            message.Duration = 4000;

            editModel.CreatedBy = userName;
            if (isValid_id && isValid_name)
            {
                if (editModel.Id > 0)
                {
                    editModel.ModifiedBy = userName;
                }
                try
                {
                    var result = await _classsService.SaveAsync(editModel, token);

                    if (result.ResponseCode == 200 && result.Data == true)
                    {
                        Cancel();
                        message.Severity = NotificationSeverity.Success;
                        message.Summary = Constants.Message.Successfully;

                        await grid.Reload();
                    }
                    else
                    {
                        Cancel();
                        message.Severity = NotificationSeverity.Error;
                        message.Summary = Constants.Message.Fail;

                    }
                    message.Detail = result.ResponseMessage;
                    message.Duration = 4000;
                }
                catch (Exception)
                {
                    Cancel();
                    message.Severity = NotificationSeverity.Error;
                    message.Summary = Constants.Message.Fail;
                    message.Detail = "Mã lớp đã tồn tại";

                }
            }
            else
            {
                Cancel();
                message.Severity = NotificationSeverity.Error;
                message.Summary = Constants.Message.Fail;
                message.Detail = Constants.Message.Validation;
                await grid.Reload();
            }    
            _notificationService.Notify(message);

        }

    }
}
