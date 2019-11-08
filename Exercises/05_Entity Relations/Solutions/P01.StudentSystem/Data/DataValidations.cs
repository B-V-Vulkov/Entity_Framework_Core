namespace P01_StudentSystem.Data
{
    public static class DataValidations
    {
        public static class Student
        {
            public const int NameMaxLength = 100;

            public const string PhoneNumberType = "CHAR(10)";
        }

        public static class Course
        {
            public const int NameMaxLength = 80;
        }

        public static class Resource
        {
            public const int NameMaxLength = 50;
        }
    }
}
