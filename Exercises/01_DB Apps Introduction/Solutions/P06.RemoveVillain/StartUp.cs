namespace P06.RemoveVillain
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            int villianId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.CoonnectionString;
                connection.Open();

                string selectQuery = "SELECT Name FROM Villains WHERE Id = @villainId";

                string villianName = string.Empty;

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villianId);
                    villianName = (string)command.ExecuteScalar();
                }

                if (villianName == null)
                {
                    Console.WriteLine("No such villain was found.");
                    return;
                }

                string deleteMinionsVillainsQuery = @"DELETE FROM MinionsVillains 
                                                    WHERE VillainId = @villainId";

                string deleteVillianQuery = @"DELETE FROM Villains
                                            WHERE Id = @villainId";

                int deletedMinions = 0;

                using (SqlCommand deleteCommand = new SqlCommand(deleteMinionsVillainsQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@villainId", villianId);
                    deletedMinions = deleteCommand.ExecuteNonQuery();
                }

                using (SqlCommand deleteCommand = new SqlCommand(deleteVillianQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@villainId", villianId);
                    deleteCommand.ExecuteNonQuery();
                }

                Console.WriteLine($"{villianName} was deleted.");
                Console.WriteLine($"{deletedMinions} minions were released.");

            }
        }
    }
}
