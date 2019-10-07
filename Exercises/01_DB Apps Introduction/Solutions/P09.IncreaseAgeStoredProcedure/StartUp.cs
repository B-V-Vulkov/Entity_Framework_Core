namespace P09.IncreaseAgeStoredProcedure
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            int minionId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.CoonnectionString;
                connection.Open();

                string execQuery = "EXEC usp_GetOlder @Id";

                using (SqlCommand command = new SqlCommand(execQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", minionId);
                    command.ExecuteScalar();
                }

                string selectQuert = "SELECT Name, Age FROM Minions WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(selectQuert, connection) )
                {
                    command.Parameters.AddWithValue("@Id", minionId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Minion minion = new Minion();
                            minion.Name = (string)reader["Name"];
                            minion.Age = (int)reader["Age"];

                            Console.WriteLine(minion);
                        }
                    }
                }
            }
        }
    }
}
