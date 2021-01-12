using System;

namespace EarthLink_Test.Dtos
{
    public class ArtistDto
    {
        public string Artist { set; get; }
        public string Title { set; get; }
        public string Album { set; get; }
        public DateTime ReleaseDate { set; get; }
        public string Label { set; get; }
        public bool Underground { set; get; }
    }
}