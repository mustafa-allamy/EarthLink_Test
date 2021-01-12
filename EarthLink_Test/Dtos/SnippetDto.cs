using EarthLink_Test.Enum;
using Google.Apis.YouTube.v3.Data;

namespace EarthLink_Test.Dtos
{
    public class SnippetDto
    {
        public string Id { set; get; }
        public string Title { set; get; }
        public ThumbnailDetails Thumbnail { set; get; }
        public YoutubeEnums Kind { set; get; }
    }
}