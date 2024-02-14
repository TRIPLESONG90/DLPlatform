namespace PLPlatform.Core.Models
{
    public class ClassColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
    public class ClassInfo
    {
        public string Name { get; set; }
        public ClassColor Color { get; set; }
    }

}
