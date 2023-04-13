using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.AspNetCore.Components;
using ProjectManager.Admin.Data;
using ProjectManager.Shared.Model.ViewModel;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace ProjectManager.Admin.Pages.Intern
{
    
        public class InternModalBase : CommonComponentBase
        {
            [Parameter]
            public RadzenDataGrid<InternViewModel> grid { get; set; }

            [Parameter]
            public InternViewModel internViewModel { get; set; } = new InternViewModel();
            public bool isLoading;
            public bool isShow;
            protected override async Task OnInitializedAsync()
            {
                isLoading = true;
                if (internViewModel.Id > 0)
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
