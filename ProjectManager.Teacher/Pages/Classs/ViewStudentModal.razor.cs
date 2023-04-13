using Microsoft.AspNetCore.Components;
using ProjectManager.Shared.Model.ViewModel;
using ProjectManager.Teacher.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Teacher.Pages.Classs
{
    public class ViewStudentModalBase : CommonComponentBase
    {
        [Parameter] public long classsId { get; set; }
        public IEnumerable<StudentViewModel> listStudent { get; set; }
        public StudentViewModel student { get; set; } = new StudentViewModel();
        public bool isLoading;
        public bool isShow;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            var data = await _studentService.GetStudentByClasssAsync(classsId, token);
            listStudent = data.Data;

            if (listStudent.Count() > 0)
            {
                isShow = true;
                student = listStudent.FirstOrDefault();
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

        public async Task HandleClickUsername(StudentViewModel item)
        {
            isLoading = true;
            student = item;
            await Delay();
            isLoading = false;
        }
    }
}
