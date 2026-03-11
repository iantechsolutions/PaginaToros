using PaginaToros.Shared.Models;

namespace PaginaToros.Client.Helpers
{
    public static class SocioDisplayExtensions
    {
        public static string CodigoVisible(this SocioDTO? socio)
        {
            if (!string.IsNullOrWhiteSpace(socio?.Codpos2))
            {
                return socio.Codpos2.Trim();
            }

            return socio?.Scod?.Trim() ?? string.Empty;
        }

        public static string CodigoVisible(this Socio? socio)
        {
            if (!string.IsNullOrWhiteSpace(socio?.Codpos2))
            {
                return socio.Codpos2.Trim();
            }

            return socio?.Scod?.Trim() ?? string.Empty;
        }
    }
}
