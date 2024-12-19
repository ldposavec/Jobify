namespace Jobify.Api.Service
{
    public static class ApiRoutes
    {
        public static class Employer
        {
            public const string Base = "api/Employer";
            public const string Register = $"{Base}/register";
            public const string JWT = $"{Base}/generate-jwt";
        }        
        
        public static class Student
        {
            public const string Base = "api/Student";
            public const string Register = $"{Base}/register";
        }        
        
        public static class UserType
        {
            public const string Base = "api/UserType";
            public const string Values = $"{Base}";
        }        
        
        public static class User
        {
            public const string Base = "api/User";
            public const string Login = $"{Base}/Login";
            public const string ChangePassword = $"{Base}/ChangePassword";
            public const string SendPasswordResetEmail = $"{Base}/SendPasswordResetEmail";
        }
    }
}
