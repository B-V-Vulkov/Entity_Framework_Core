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

        public static class Player
        {
            public const int NameMaxLength = 30;

            public const int SquadNumberMaxLength = 99;
        }

        public static class Position
        {
            public const int NameMaxLength = 30;
        }

        public static class PlayerStatistic
        {
            public const int ScoredGoalsMaxLength = 100;

            public const int AssistsMaxLength = 200;

            public const int MinutesPlayedMaxLength = 10000;
        }

        public static class Game
        {
            public const int HomeTeamGoalsMaxLength = 30;

            public const int AwayTeamGoalsMaxLength = 30;

            public const int HomeTeamBetRateMaxLength = 30;

            public const int AwayTeamBetRateMaxLength = 30;

            public const int DrawBetRateMaxLength = 30;

            public const int ResultMaxLength = 10;
        }

        public static class Bet
        {
            public const int AmountMaxLength = 100000;
        }

        public static class User
        {
            public const int UsernameMaxLength = 20;

            public const int PasswordMaxLength = 50;

            public const int EmailMaxLength = 50;

            public const int NameMaxLength = 50;

            public const int BalanceMaxLength = 200000;
        }
    }
}
