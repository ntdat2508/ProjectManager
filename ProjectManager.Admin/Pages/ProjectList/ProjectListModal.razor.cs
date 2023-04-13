using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Model.ViewModel;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.ProjectList
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
    }
}
