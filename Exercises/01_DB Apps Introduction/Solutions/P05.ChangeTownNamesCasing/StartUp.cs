namespace P05.ChangeTownNamesCasing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            string country = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Configuration.CoonnectionString;
                connection.Open();

                string updateQuery = @"UPDATE Towns
                                        SET Name = UPPER(Name)
                                        WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@countryName", country);
                    int numberOfrows = updateCommand.ExecuteNonQuery();

                    if (numberOfrows == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        Console.WriteLine($"{numberOfrows} town names were affected.");
                        List<string> townNames = GetTownName(country, connection);
                        Console.WriteLine("[" + string.Join(", ", townNames) + "]");
                    }
                }
            }
        }

        private static List<string> GetTownName(string country, SqlConnection connection)
        {
            string selectQuery = @"SELECT t.Name 
                                        FROM Towns as t
                                        JOIN Countries AS c ON c.Id = t.CountryCode
                                        WHERE c.Name = @countryName";

            using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
            {
                selectCommand.Parameters.AddWithValue("@countryName", country);

                List<string> townNames = new List<string>();

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        townNames.Add((string)reader[0]);
                    }

                    return townNames;
                }
            }
        }
    }
}
