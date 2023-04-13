using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Authentication.Commands;
using ProjectManager.Authentication.Commands.Factory;
using ProjectManager.Authentication.Model;
using ProjectManager.Repository;
using ProjectManager.Services.Api;
using System.Linq;
using System.Reflection;

namespace ProjectManager.Ioc
{
    public class ComponentRegistrar : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(new[] { Assembly.GetAssembly(typeof(IDepartmentService)) })
                .Where(x => x.IsClass && x.Name.EndsWith("Service"))
                .As(t => t.GetInterfaces().Any() ? t.GetInterfaces()[0] : t);

            builder.RegisterAssemblyTypes(new[] { Assembly.GetAssembly(typeof(IDapperRepository)) })
            .Where(x => x.IsClass && x.Name.EndsWith("Repository"))
            .As(t => t.GetInterfaces().Any() ? t.GetInterfaces()[0] : t);

            builder.RegisterAssemblyTypes(new[] { Assembly.GetAssembly(typeof(ILoginCommand)) })
          .Where(x => x.IsClass && x.Name.EndsWith("Command"))
          .As(t => t.GetInterfaces().Any() ? t.GetInterfaces()[0] : t);

            builder.RegisterAssemblyTypes(new[] { Assembly.GetAssembly(typeof(JwtIssuerOptions)) })
          .Where(x => x.IsClass && x.Name.EndsWith("Options"))
          .As(t => t.GetInterfaces().Any() ? t.GetInterfaces()[0] : t);

            builder.RegisterAssemblyTypes(new[] { Assembly.GetAssembly(typeof(IJwtFactory)) })
              .Where(x => x.IsClass && x.Name.EndsWith("Factory"))
              .As(t => t.GetInterfaces().Any() ? t.GetInterfaces()[0] : t);

            //----------------------Common-----------------------------------------------
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            builder.RegisterType<DbContext>().As<DbContext>();
        }
    }
}
