namespace ChatGPT.API.Models
{
    public class AuthOptions
    {
        public const string CookieKey = ".AspNetCore.BuildingInspection.Id";
        public const string DemoModeEnabled = "BuildingInspectionAPI.DemoModeEnabled";
        public const string Issuer = "BuildingInspectionAPI";
        public const int Lifetime = 60 * 24 * 1;//one day in minutes //60 * 24 * 360; // one year in minutes
        public const string SecurityKey = "a12b1917-b3d9-46be-a853-a65c3bb963c6";
        public const string BranchId = "BranchId";
        public const string CORSPolicy = "BuildingInspectionCors";
        public const string AuthPolicy = "BuildingInspectionUserPolicy";
        public const string HashKey = "BuildingInspectionENCRYPTPASSWORD9B522E695D4F2"; //32
        public const string EmployeeId = "EmployeeId";
        public const string EmployeeCode = "EmployeeCode";
        public const string Environment = "Environment";
        public const string Priority = "Priority";
    }
}
