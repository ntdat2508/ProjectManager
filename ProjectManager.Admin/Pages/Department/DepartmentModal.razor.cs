using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Department
{
    public class DepartmentModalBase : CommonComponentBase
    {
        [Parameter] public RadzenDataGrid<DepartmentViewModel> grid { get; set; }
        [Parameter] public DepartmentViewModel departmentViewModel { get; set; }
        public Entity.Department editModel { get; set; } = new Entity.Department();
        public bool isLoading;
        public bool isShow;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            if (departmentViewModel.Id > 0)
            {
                editModel.ID_Department=departmentViewModel.ID_Department; 
                editModel.Id = departmentViewModel.Id;
                editModel.Name = departmentViewModel.Name;
                editModel.FoundingDate = departmentViewModel.FoundingDate;
                editModel.Discriptions = departmentViewModel.Discriptions;
                editModel.CreatedBy = departmentViewModel.CreatedBy;
                editModel.CreatedDate = departmentViewModel.CreatedDate;
                editModel.ModifiedBy = departmentViewModel.ModifiedBy;
                editModel.ModifiedDate = departmentViewModel.ModifiedDate;
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
            var message = new NotificationMessage();
            message.Duration = 4000;

            editModel.CreatedBy = userName;
            if (editModel.Id > 0)
            {
                editModel.ModifiedBy = userName;
            }

            var result = await _departmentService.SaveAsync(editModel, token);

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
            message.Duration = 4000;
            _notificationService.Notify(message);
        }
    }
}
