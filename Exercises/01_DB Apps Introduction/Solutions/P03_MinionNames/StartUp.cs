namespace P03.MinionNames
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.CoonnectionString;
                connection.Open();

                string villainNameQuery = "SELECT Name FROM Villains WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(villainNameQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    string villianName = (string)command.ExecuteScalar();

                    if (villianName == null)
                    {
                        Console.WriteLine("No villain with ID 10 exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villain: {villianName}");

                }

                string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                                m.Name, 
                                                m.Age
                                            FROM MinionsVillains AS mv
                                            JOIN Minions As m ON mv.MinionId = m.Id
                                            WHERE mv.VillainId = @Id
                                            ORDER BY m.Name";
                using (SqlCommand command = new SqlCommand(minionsQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long rowNumber = (long)reader["RowNum"];
                            string name = (string)reader["Name"];
                            int age = (int)reader["Age"];

                            Console.WriteLine($"{rowNumber}. {name} {age}");
                        }

                        if (!reader.HasRows)
                        {
                            Console.WriteLine("(no minions)");
                            return;
                        }
                    }
                }
            }
        }
    }
}
