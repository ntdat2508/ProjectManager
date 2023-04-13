using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.ViewModel;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Teacher
{
    public class TeacherModalBase : CommonComponentBase
    {
        [Parameter] public RadzenDataGrid<TeacherViewModel> grid { get; set; }
        [Parameter] public TeacherViewModel teacherViewModel { get; set; }
        [Parameter] public IEnumerable<Entity.Department> listDepartment { get; set; }
        [Parameter] public IEnumerable<Entity.Specialized> listSpecialized { get; set; }
        [Parameter] public IEnumerable<Teacher.Index.genderoj> listGender { get; set; }

        public Entity.Teacher editModel { get; set; } = new Entity.Teacher();
        public bool isLoading;
        public bool isShow;
   
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            if (teacherViewModel.Id > 0)
            {
             

                editModel.ID_Teacher = teacherViewModel.ID_Teacher;
                editModel.Id = teacherViewModel.Id;
                editModel.Username = teacherViewModel.Username;
                editModel.Name = teacherViewModel.Name;
                editModel.PhoneNumber = teacherViewModel.PhoneNumber;
                editModel.Email = teacherViewModel.Email;
                editModel.Address = teacherViewModel.Address;
                editModel.DateOfBirth = teacherViewModel.DateOfBirth;
                editModel.Gender = teacherViewModel.Gender;
                editModel.DepartmentId = teacherViewModel.DepartmentId;
                editModel.SpecializedId = teacherViewModel.SpecializedId;
                editModel.CreatedBy = teacherViewModel.CreatedBy;
                editModel.CreatedDate = teacherViewModel.CreatedDate;
                editModel.ModifiedBy = teacherViewModel.ModifiedBy;
                editModel.ModifiedDate = teacherViewModel.ModifiedDate;
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
            var isNumberPhone = new Regex("^\\d{10,11}$");
            var isId = new Regex("^[a-zA-Z0-9]");
            var isName = new Regex("^[a-zA-Z\\p{L}\\s]*$");
            var isEmail = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");


            var isValid_name = isName.IsMatch(editModel.Name);
            var isValid_id = isId.IsMatch(editModel.ID_Teacher);
            var isNumcheck = isNumberPhone.IsMatch(editModel.PhoneNumber);
            var isEmailCheck = isEmail.IsMatch(editModel.Email);



            var message = new NotificationMessage();
            message.Duration = 4000;

            editModel.CreatedBy = userName;
            if (isValid_id && isValid_name && isNumcheck && isEmailCheck)
            {
                if (editModel.Id > 0)
                {
                    editModel.ModifiedBy = userName;
                }
                try
                {

                    var result = await _teacherService.SaveAsync(editModel, token);

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
