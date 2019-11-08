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

        public static class Color
        {
            public const int NameMaxLength = 30;
        }

        public static class Town
        {
            public const int NameMaxLength = 30;
        }

        public static class Country
        {
            public const int NameMaxLength = 30;
        }
    }
}
