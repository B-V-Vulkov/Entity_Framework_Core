namespace P03_FootballBetting.Data
{
    public static class DataValidations
    {
        public static class Team
        {
            public const int NameMaxLength = 30;

            public const int LogoUrlrMaxLength = 300;

            public const int InitialsMaxLength = 3;
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
