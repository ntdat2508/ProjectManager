using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Authentication.Model;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model;
using System;
using System.Text;

namespace ProjectManager.Infrastructure
{
    public static class ConfigureService
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public static void InitAuthentication(this IServiceCollection services)
        {
            var configuration = ServiceLocator.Current.GetInstance<IConfiguration>();
            // Register the ConfigurationBuilder instance of FacebookAuthSetting
            //services.Configure<FacebookAuthSettings>(configuration.GetSection(nameof(FacebookAuthSettings)));
            // Get options from app settings
            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));
            var a = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SigningKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });
            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            var appSettingOptions = configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(options =>
            {
                options.BaseUri = appSettingOptions[nameof(AppSettings.BaseUri)];

                options.Authentication = appSettingOptions[nameof(AppSettings.Authentication)];

                //Classs (Quản lý lớp)
                options.Classs_GetAll = appSettingOptions[nameof(AppSettings.Classs_GetAll)];
                options.Classs_Save = appSettingOptions[nameof(AppSettings.Classs_Save)];
                options.Classs_Delete = appSettingOptions[nameof(AppSettings.Classs_Delete)];
                options.Classs_GetAllClasss = appSettingOptions[nameof(AppSettings.Classs_GetAllClasss)];

                //Intern
                options.Intern_GetAll = appSettingOptions[nameof(AppSettings.Intern_GetAll)];
                options.Intern_Save = appSettingOptions[nameof(AppSettings.Intern_Save)];
                options.Intern_Delete = appSettingOptions[nameof(AppSettings.Intern_Delete)];
                options.Intern_Mark = appSettingOptions[nameof(AppSettings.Intern_Mark)];
                options.Intern_GetAllIntern = appSettingOptions[nameof(AppSettings.Intern_GetAllIntern)];

                //Department (Quản lý khoa)
                options.Department_GetAll = appSettingOptions[nameof(AppSettings.Department_GetAll)];
                options.Department_Save = appSettingOptions[nameof(AppSettings.Department_Save)];
                options.Department_Delete = appSettingOptions[nameof(AppSettings.Department_Delete)];
                options.Department_GetAllDepartment = appSettingOptions[nameof(AppSettings.Department_GetAllDepartment)];

                //ProjectList (Danh sách đồ án)
                options.ProjectList_GetAll = appSettingOptions[nameof(AppSettings.ProjectList_GetAll)];
                options.ProjectList_Save = appSettingOptions[nameof(AppSettings.ProjectList_Save)];
                options.ProjectList_Delete = appSettingOptions[nameof(AppSettings.ProjectList_Delete)];
                options.ProjectList_Mark = appSettingOptions[nameof(AppSettings.ProjectList_Mark)];
                options.ProjectList_GetAllProjectList = appSettingOptions[nameof(AppSettings.ProjectList_GetAllProjectList)];

                //SchoolYear (Quản lý niên khóa)
                options.SchoolYear_GetAll = appSettingOptions[nameof(AppSettings.SchoolYear_GetAll)];
                options.SchoolYear_Save = appSettingOptions[nameof(AppSettings.SchoolYear_Save)];
                options.SchoolYear_Delete = appSettingOptions[nameof(AppSettings.SchoolYear_Delete)];
                options.SchoolYear_GetAllSchoolYear = appSettingOptions[nameof(AppSettings.SchoolYear_GetAllSchoolYear)];

                //Specialized (Quản lý chuyên ngành)
                options.Specialized_GetAll = appSettingOptions[nameof(AppSettings.Specialized_GetAll)];
                options.Specialized_Save = appSettingOptions[nameof(AppSettings.Specialized_Save)];
                options.Specialized_Delete = appSettingOptions[nameof(AppSettings.Specialized_Delete)];
                options.Specialized_GetAllSpecialized = appSettingOptions[nameof(AppSettings.Specialized_GetAllSpecialized)];

                //Student (Quản lý sinh viên)
                options.Student_GetAll = appSettingOptions[nameof(AppSettings.Student_GetAll)];
                options.Student_Save = appSettingOptions[nameof(AppSettings.Student_Save)];
                options.Student_Delete = appSettingOptions[nameof(AppSettings.Student_Delete)];
                options.Student_GetStudentByClasss = appSettingOptions[nameof(AppSettings.Student_GetStudentByClasss)];
                options.Student_GetAllStudent = appSettingOptions[nameof(AppSettings.Student_GetAllStudent)];
                options.Student_GetSelectAllByUsername = appSettingOptions[nameof(AppSettings.Student_GetSelectAllByUsername)];

                //Teacher (Quản lý giáo viên)
                options.Teacher_GetAll = appSettingOptions[nameof(AppSettings.Teacher_GetAll)];
                options.Teacher_Save = appSettingOptions[nameof(AppSettings.Teacher_Save)];
                options.Teacher_Delete = appSettingOptions[nameof(AppSettings.Teacher_Delete)];
                options.Teacher_GetAllTeacher = appSettingOptions[nameof(AppSettings.Teacher_GetAllTeacher)];
                options.Teacher_GetTeacherBySpecialized = appSettingOptions[nameof(AppSettings.Teacher_GetTeacherBySpecialized)];

                //TrainingSystem (Quản lý hệ đào tạo)
                options.TrainingSystem_GetAll = appSettingOptions[nameof(AppSettings.TrainingSystem_GetAll)];
                options.TrainingSystem_Save = appSettingOptions[nameof(AppSettings.TrainingSystem_Save)];
                options.TrainingSystem_Delete = appSettingOptions[nameof(AppSettings.TrainingSystem_Delete)];
                options.TrainingSystem_GetAllTrainingSystem = appSettingOptions[nameof(AppSettings.TrainingSystem_GetAllTrainingSystem)];
            });
        }
    }
}
