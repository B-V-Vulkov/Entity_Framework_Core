namespace P04.AddMinion
{
    using System;
    using System.Data.SqlClient;

    class StartUp
    {
        static void Main()
        {
            string[] minionInfo = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string[] villianInfo = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string minionTown = minionInfo[3];
            string villianName = villianInfo[1];

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.CoonnectionString;
                connection.Open();

                int? townId = GetTownByName(connection, minionTown);
                if (townId == null)
                {
                    AddTown(connection, minionTown);
                    Console.WriteLine($"Town {minionTown} was added to the database.");
                }

                AddMinion(connection, minionName, minionAge, townId);
                int? minionId = GetMinionIdByName(connection, minionName);

                int? villianId = GetVillainByName(connection, villianName);
                if (villianId == null)
                {
                    AddVillian(connection, villianName);
                    Console.WriteLine($"Villain {villianName} was added to the database.");
                    villianId = GetVillainByName(connection, villianName);
                }

                string insetrMinionVillian = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

                using (SqlCommand command = new SqlCommand(insetrMinionVillian, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villianId);
                    command.Parameters.AddWithValue("@minionId", minionId);
                    command.ExecuteNonQuery();

                    Console.WriteLine($"Successfully added {minionName} to be minion of {villianName}.");
                }
            }
        }

        private static void AddMinion(SqlConnection connection, string minionName, int age, int? townId)
        {
            string insertMinionnQuery = "INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinionnQuery, connection))
            {
                command.Parameters.AddWithValue("@nam", minionName);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", townId);
                command.ExecuteNonQuery();
            }
        }

        private static void AddVillian(SqlConnection connection, string villianName)
        {
            string insertVillianQuery = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
            ExecuteNonQuery(connection, villianName, insertVillianQuery, "@villainName");
        }

        private static void AddTown(SqlConnection connection, string townName)
        {
            string insertTownQuery = "INSERT INTO Towns (Name) VALUES (@townName)";
            ExecuteNonQuery(connection, townName, insertTownQuery, "@townName");
        }

        private static int? GetMinionIdByName(SqlConnection connection, string minionName)
        {
            string villianIdQuery = "SELECT Id FROM Minions WHERE Name = @Name";
            return ExecuteScalar(connection, minionName, villianIdQuery, "@Name");
        }

        private static int? GetVillainByName(SqlConnection connection, string villianName)
        {
            string villianIdQuery = "SELECT Id FROM Villains WHERE Name = @Name";
            return ExecuteScalar(connection, villianName, villianIdQuery, "@Name");
        }

        private static int? GetTownByName(SqlConnection connection, string townName)
        {
            string townIdQuery = "SELECT Id FROM Towns WHERE Name = @townName";
            return ExecuteScalar(connection, townName, townIdQuery, "@townName");
        }

        private static int? ExecuteScalar(SqlConnection connection, string name, string query, string parameter)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(parameter, name);
                return (int?)command.ExecuteScalar();
            }
        }

        private static void ExecuteNonQuery(SqlConnection connection, string name, string insertQuery, string parameter)
        {
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue(parameter, name);
                command.ExecuteNonQuery();
            }
        }
    }
}
