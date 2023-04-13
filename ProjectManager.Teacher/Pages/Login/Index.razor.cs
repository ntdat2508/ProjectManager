using ProjectManager.Authentication.Model;
using ProjectManager.Shared.Common.Enum;
using ProjectManager.Shared.Constants;
using ProjectManager.Teacher.Data;
using Radzen;
using System.Threading.Tasks;

namespace ProjectManager.Teacher.Pages.Login
{
    public class IndexBase : CommonComponentBase
    {
        public LoginRequest request { get; set; } = new LoginRequest();
        public bool isLoading;

        protected override void OnInitialized()
        {
            request.UserName = string.Empty;
            request.Role = RoleEnum.Teacher.ToString();
        }

        public async Task OnSubmit()
        {
            isLoading = true;
            var result = await _authenticationService.LoginAsync(request);
            var message = new NotificationMessage();
            if (result.Code == 200)
            {
                token = result.Data.Token;
                userName = result.Data.Username;
                expireTime = result.Data.ExpireTime;
                _navigationManager.NavigateTo("/");
            }
            else
            {
                message.Severity = NotificationSeverity.Error;
                message.Summary = Constants.Message.Fail;
                message.Detail = result.Message;
                message.Duration = 4000;
                _notificationService.Notify(message);
            }
            await Delay();
            isLoading = false;
        }

        public void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {

        }
    }
}
