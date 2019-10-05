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

                string villianIdQuery = "SELECT Id FROM Villains WHERE Name = @Name";

                using (SqlCommand command = new SqlCommand(villianIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", villianName);
                    int villianId = (int)command.ExecuteScalar();


                    Console.WriteLine(villianId);
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }
    }
}
