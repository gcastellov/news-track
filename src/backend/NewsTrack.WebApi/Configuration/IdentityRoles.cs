using System;
using NewsTrack.Identity;

namespace NewsTrack.WebApi.Configuration
{
    internal class IdentityRoles
    {
        public const string Administrator = "Administrator";
        public const string Contributor = "Contributor";
        public const string Regular = "Regular";

        public static string ToRole(IdentityTypes type)
        {
            switch (type)
            {
                case IdentityTypes.Admin:
                    return Administrator;
                case IdentityTypes.Contributor:
                    return Contributor;
                case IdentityTypes.Regular:
                    return Regular;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}