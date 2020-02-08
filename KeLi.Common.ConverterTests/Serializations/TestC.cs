namespace KeLi.Common.Converter.Serializations.Tests
{
    public class TestC
    {
        public TestC()
        {
            Id = 0;
            Name = null;
        }

        public TestC(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}