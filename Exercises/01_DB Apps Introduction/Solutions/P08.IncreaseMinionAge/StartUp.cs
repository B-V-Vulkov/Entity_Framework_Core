namespace P08.IncreaseMinionAge
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            int[] minionIds = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.CoonnectionString;
                connection.Open();

                string updateQuery = @"UPDATE Minions
                                    SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                    WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    for (int i = 0; i < minionIds.Length; i++)
                    {
                        command.Parameters.AddWithValue("@Id", minionIds[i]);
                        command.ExecuteScalar();
                    }
                }

                string selectQuery = @"SELECT Name, Age FROM Minions";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    List<Minion> minions = new List<Minion>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Minion minion = new Minion();
                            minion.Name = (string)reader["Name"];
                            minion.Age = (int)reader["Age"];
                            minions.Add(minion);
                        }
                    }

                    foreach (var minion in minions)
                    {
                        Console.WriteLine(minion);
                    }
                }
            }
        }
    }
}
