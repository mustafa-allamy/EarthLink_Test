using System.Collections.Generic;
using System.Threading.Tasks;
using EarthLink_Test.Dtos;

namespace EarthLink_Test.Interfaces.Services
{
    public interface IYoutubeService
    {
        Task<bool> DownloadVideo(string url, string fileName);
        Task ConvertToMp3(string fileName);
        Task Cut1Min(string fileName);
        Task<ArtistDto> RecognizeSong(string fileName);
        Task<List<SnippetDto>> GetArtistSongs(string artistName);
        Task RemoveFiles(string fileName);
    }
}