namespace DemoDictionary.Data.Models
{
    using Enumerations;

    public class Word
    {
        public int Id { get; }
        public TypeOfWord Type { get; }

        public Word(int id, TypeOfWord type)
        {
            Id = id;
            Type = type;
        }
    }
}
