using System;
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
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using ProjectManager.Entity;
using BlazorInputFile;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Model.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Specialized
{

    public class ViewTeacherModalBase : CommonComponentBase
    {
        [Parameter]
        public long specializedId { get; set; }

        public IEnumerable<TeacherViewModel> listTeacher { get; set; }

        public TeacherViewModel teacher { get; set; } = new TeacherViewModel();
        public bool isLoading;
        public bool isShow;
        protected override async Task OnInitializedAsync()
        {
           
        }

        public void Cancel()
        {
            _dialogService.Close(true);
        }

        public async Task HandleClickUsername(TeacherViewModel item)
        {
            isLoading = true;
            teacher = item;
            await Delay();
            isLoading = false;
        }
    }

}