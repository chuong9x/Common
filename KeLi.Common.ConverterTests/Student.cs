using System;

namespace KeLi.Common.ConverterTests
{
    [Serializable]
    public class Student
    {
        public Student()
        {
            Id = 0;
            Name = null;
        }

        public Student(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}