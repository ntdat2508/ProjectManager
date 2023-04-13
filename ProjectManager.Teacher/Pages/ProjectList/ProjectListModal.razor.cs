using Microsoft.AspNetCore.Components;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using ProjectManager.Teacher.Data;
using Radzen;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace ProjectManager.Teacher.Pages.ProjectList
{
    public class ProjectListModalBase : CommonComponentBase
    {
        [Parameter] public RadzenDataGrid<ProjectListViewModel> grid { get; set; }
        [Parameter] public ProjectListViewModel projectListViewModel { get; set; } = new ProjectListViewModel();
        public bool isLoading;
        public bool isShow;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            if (projectListViewModel.Id > 0)
            {
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

        public async Task OnSubmit()
        {
            var message = new NotificationMessage();
            message.Duration = 4000;

            var model = new Entity.ProjectList
            {
                Id = projectListViewModel.Id,
                ModifiedBy = userName,
                Point = projectListViewModel.Point
            };
            var result = await _projectListService.MarkAsync(model, token);

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
