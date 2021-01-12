using EarthLink_Test.Enum;
using Google.Apis.YouTube.v3.Data;

namespace EarthLink_Test.Dtos
{
    public class SnippetDto
    {
        public string id { set; get; }
        public string title { set; get; }
        public ThumbnailDetails thumbnail { set; get; }
        public YoutubeEnums kind { set; get; }
    }
}