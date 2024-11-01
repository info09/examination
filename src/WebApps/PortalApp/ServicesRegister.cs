using PortalApp.Services;
using PortalApp.Services.Interfaces;

namespace PortalApp
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IExamService, ExamService>();
        }
    }
}
