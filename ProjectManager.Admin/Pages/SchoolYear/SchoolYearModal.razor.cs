using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.SchoolYear
{
    public class SchoolYearModalBase : CommonComponentBase
    {
        [Parameter] public RadzenDataGrid<SchoolYearViewModel> grid { get; set; }
        [Parameter] public SchoolYearViewModel schoolYearViewModel { get; set; }
        public Entity.SchoolYear editModel { get; set; } = new Entity.SchoolYear();
        public bool isLoading;
        public bool isShow;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            if (schoolYearViewModel.Id > 0)
            {
                editModel.Id = schoolYearViewModel.Id;
                editModel.Name = schoolYearViewModel.Name;
                editModel.CreatedBy = schoolYearViewModel.CreatedBy;
                editModel.CreatedDate = schoolYearViewModel.CreatedDate;
                editModel.ModifiedBy = schoolYearViewModel.ModifiedBy;
                editModel.ModifiedDate = schoolYearViewModel.ModifiedDate;
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

            var result = await _schoolYearService.SaveAsync(editModel, token);

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
