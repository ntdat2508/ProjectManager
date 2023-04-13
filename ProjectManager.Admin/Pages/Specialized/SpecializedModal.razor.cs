using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Specialized
{
    public class SpecializedModalBase : CommonComponentBase
    {
        [Parameter] public RadzenDataGrid<SpecializedViewModel> grid { get; set; }
        [Parameter] public SpecializedViewModel specializedViewModel { get; set; }
        public IEnumerable<DepartmentViewModel> listDepartment { get; set; }
        public Entity.Specialized editModel { get; set; } = new Entity.Specialized();
        public bool isLoading;
        public bool isShow;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            var department = await _departmentService.GetAllDepartmentAsync(token);
            listDepartment = department.Data;

            if (specializedViewModel.Id > 0)
            {
                editModel.Id = specializedViewModel.Id;
                editModel.ID_Specialized = specializedViewModel.ID_Specialized;
                editModel.Name = specializedViewModel.Name;
                editModel.DepartmentId = specializedViewModel.DepartmentId;
                editModel.Discriptions = specializedViewModel.Discriptions;
                editModel.CreatedBy = specializedViewModel.CreatedBy;
                editModel.CreatedDate = specializedViewModel.CreatedDate;
                editModel.ModifiedBy = specializedViewModel.ModifiedBy;
                editModel.ModifiedDate = specializedViewModel.ModifiedDate;
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


            var isValid_id = isId.IsMatch(editModel.ID_Specialized);
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
                    var result = await _specializedService.SaveAsync(editModel, token);

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
