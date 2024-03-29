﻿namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Performer")]
    public class ImportPerformerDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        [XmlElement("LastName")]
        public string LastName { get; set; }

        [Range(18, 70)]
        [XmlElement("Age")]
        public int Age { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlElement("NetWorth")]
        public decimal NetWorth { get; set; }

        [XmlArray("PerformersSongs")]
        public ImportPerformerSongDto[] PerformersSongs { get; set; }
    }

    [XmlType("Song")]
    public class ImportPerformerSongDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
