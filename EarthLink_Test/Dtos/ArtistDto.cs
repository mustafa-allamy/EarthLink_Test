using System;

namespace EarthLink_Test.Dtos
{
    public class ArtistDto
    {
        public string artist { set; get; }
        public string title { set; get; }
        public string album { set; get; }
        public DateTime release_date { set; get; }
        public string label { set; get; }
        public bool underground { set; get; }
    }
}