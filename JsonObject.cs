namespace JsonToText
{
    public class JsonObject
    {
        public string Locale { get; set; }
        public string Description { get; set; }
        public BoundingPoly BoundingPoly { get; set; }
    }
    public class BoundingPoly
    {
        public List<Vertices> vertices { get; set; }
    }
    public class Vertices
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class WordWithCoordinates
    {
        //Kelimelerin sadece sol üst noktalarının koordinatları ile kıyaslayabiliriz.
        public string Description { get; set; }
        public int TopLeftX { get; set; }
        public int TopLeftY { get; set;}
    }
}
