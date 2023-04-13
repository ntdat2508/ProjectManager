using Microsoft.Extensions.DependencyInjection;

namespace ProjectManager.Shared.Common.Configuration
{
    public static class ConfigurePolicy
    {
        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AccessApp",
                    policy => policy.RequireAssertion(c =>
                        c.User.HasClaim("access-agm-test", "access") ||
                        c.User.IsInRole("Administrador")));
            });

        }
    }
}
