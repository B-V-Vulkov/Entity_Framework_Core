namespace P08.IncreaseMinionAge
{
    public class Minion
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"{this.Name} {this.Age}";
        }
    }
}
