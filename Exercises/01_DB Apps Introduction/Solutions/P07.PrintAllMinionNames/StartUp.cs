namespace P07.PrintAllMinionNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            string selectQuery = "SELECT Name FROM Minions";

            List<string> minions = new List<string>();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.CoonnectionString;
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minions.Add((string)reader[0]);
                        }
                    }
                }
            }

            int count = 0;

            while (minions.Count != 0)
            {
                if (count % 2 == 0)
                {
                    Console.WriteLine(minions[0]);
                    minions.RemoveAt(0);
                }
                else
                {
                    Console.WriteLine(minions[minions.Count - 1]);
                    minions.RemoveAt(minions.Count - 1);
                }
                count++;
            }
        }
    }
}
