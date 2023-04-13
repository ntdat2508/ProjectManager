using Microsoft.AspNetCore.Components;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using ProjectManager.Teacher.Data;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManager.Teacher.Pages.Student
{
    public class StudentModalBase : CommonComponentBase
    {
        [Parameter] public RadzenDataGrid<StudentViewModel> grid { get; set; }
        [Parameter] public StudentViewModel studentViewModel { get; set; }
        [Parameter] public IEnumerable<Entity.Department> listDepartment { get; set; }
        [Parameter] public IEnumerable<Entity.Specialized> listSpecialized { get; set; }
        [Parameter] public IEnumerable<Entity.Classs> listClasss { get; set; }
        public IEnumerable<Entity.TrainingSystem> listTrainingSystem { get; set; }
        public Entity.Student editModel { get; set; } = new Entity.Student();
        public bool isLoading;
        public bool isShow;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            var trainingSystem = await _trainingSystemService.GetAllTrainingSystemAsync(token);
            listTrainingSystem = trainingSystem.Data;

            if (studentViewModel.Id > 0)
            {
                editModel.Id = studentViewModel.Id;
                editModel.Username = studentViewModel.Username;
                editModel.Name = studentViewModel.Name;
                editModel.PhoneNumber = studentViewModel.PhoneNumber;
                editModel.Email = studentViewModel.Email;
                editModel.Address = studentViewModel.Address;
                editModel.DateOfBirth = studentViewModel.DateOfBirth;
                editModel.SpecializedId = studentViewModel.SpecializedId;
                editModel.TrainingSystemId = studentViewModel.TrainingSystemId;
                editModel.DepartmentId = studentViewModel.DepartmentId;
                editModel.ClasssId = studentViewModel.ClasssId;
                editModel.CreatedBy = studentViewModel.CreatedBy;
                editModel.CreatedDate = studentViewModel.CreatedDate;
                editModel.ModifiedBy = studentViewModel.ModifiedBy;
                editModel.ModifiedDate = studentViewModel.ModifiedDate;
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

            var result = await _studentService.SaveAsync(editModel, token);

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
