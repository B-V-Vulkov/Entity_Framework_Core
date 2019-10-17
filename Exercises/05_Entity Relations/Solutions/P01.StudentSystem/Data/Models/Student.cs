﻿namespace P01_StudentSystem.Data.Models
{
    using System;

    public class Student
    {
        public int StudentId { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? RegisteredOn { get; set; }

        public DateTime Birthday { get; set; }
    }
}