using AutoMapper;
using Smart_Librarian.MappingProfile;

namespace Smart_Librarian.Config.Extensions
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            var cfg = new MapperConfigurationExpression();

            cfg.AddProfile<BookMappingProfile>();
            cfg.AddProfile<LoanMappingProfile>();
            cfg.AddProfile<MemberMappingProfile>();

            var mapperConfig = new MapperConfiguration(cfg);
            services.AddSingleton(mapperConfig.CreateMapper());

            return services;
        }
    }
}
