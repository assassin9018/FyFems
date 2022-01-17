using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyFemsApi;

public static class ConfigOptions
{
    public const string AllowedHosts = "AllowedHosts";

    public struct Sections
    {
        public const string ConnectionStrings = "ConnectionStrings";
        public const string AllowedHosts = "AllowedHosts";
        public const string Logging = "Logging";
        public const string LogLevel = "LogLevel";
    }

    public struct ConnectionStrings
    {
        public const string DefaultConnection = "DefaultConnection";
    }

    public struct JwtAuthOptions
    {
        private const string JwtAuthOptionsSection = "JwtAuthOptions";
        private const string ISSUER = "ISSUER";
        private const string AUDIENCE = "AUDIENCE";
        private const string KEY = "KEY";

        public string Issuer { get; }
        public string Audience { get; }
        public string Key { get; }

        /// <summary>
        /// Не поддерживаемый конструктор. Ожидается вызов конструктора с параметром IConfiguration.
        /// </summary>
        public JwtAuthOptions()
        {
            throw new NotSupportedException();
        }

        public JwtAuthOptions(IConfiguration configuration)
        {
            var section = configuration.GetSection(JwtAuthOptionsSection);
            Issuer = section.GetValue<string>(ISSUER);
            Audience = section.GetValue<string>(AUDIENCE);
            Key = section.GetValue<string>(KEY);
        }

        public SecurityKey GetSymmetricKey() 
            => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }

    /// <summary>
    /// LogLevel section. Subsection of Logging.
    /// </summary>
    public struct LogLevel
    {
        public const string Default = "Default";
        public const string MicrosoftAspNetCore = "Microsoft.AspNetCore";
    }
}
