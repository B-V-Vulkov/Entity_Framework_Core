namespace P09.IncreaseAgeStoredProcedure
{
    public class Minion
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"{this.Name} – {this.Age} years old";
        }
    }
}
