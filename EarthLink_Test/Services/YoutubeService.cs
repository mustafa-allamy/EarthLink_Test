using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using EarthLink_Test.Dtos;
using EarthLink_Test.Enum;
using EarthLink_Test.Helpers;
using EarthLink_Test.Interfaces.Services;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using NYoutubeDL;
using NYoutubeDL.Helpers;

namespace EarthLink_Test.Services
{
    public class YoutubeService : IYoutubeService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public YoutubeService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        private string _auddApiToken = "place token here";
        string _auddApiUrl = "https://api.audd.io/";
        string _auddApiMethod = "recognize";
        private string _youtubeApiKey = "place key here";
        public async Task<bool> DownloadVideo(string url, string fileName)
        {
            try
            {
                var youtubeDl = new YoutubeDL();
                youtubeDl.Options.FilesystemOptions.Output = GetDownloadDirctory() + $@"\{fileName}.mp4";
                youtubeDl.Options.VideoFormatOptions.Format = Enums.VideoFormat.mp4;
                youtubeDl.VideoUrl = url;
                youtubeDl.StandardOutputEvent += (sender, output) => Console.WriteLine(output);
                youtubeDl.StandardErrorEvent += (sender, errorOutput) => Console.WriteLine(errorOutput);
                await youtubeDl.DownloadAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task ConvertToMp3(string fileName)
        {
            Process.Start($@"{_hostEnvironment.WebRootPath}\ffmpeg\ffmpeg.exe",
                $@"-i {GetDownloadDirctory()}\{fileName}.mp4 {GetDownloadDirctory()}\{fileName}.mp3");
        }

        public async Task Cut1Min(string fileName)
        {
            Process.Start($@"{_hostEnvironment.WebRootPath}\ffmpeg\ffmpeg.exe",
                $@"-ss 0 -t 60 -i {GetDownloadDirctory()}\{fileName}.mp3 {GetDownloadDirctory()}\{fileName}Cuted.mp3");
        }

        public async Task<ArtistDto> RecognizeSong(string fileName)
        {
            RequestHelper requestHelper = new RequestHelper();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("method", _auddApiMethod);
            parameters.Add("api_token", _auddApiToken);
            string response = requestHelper.ExecuteRequestSendFile(_auddApiUrl, parameters, null, GetDownloadDirctory() + $@"\{fileName}");
            ArtistWrapperDto jsonResult = null;
            try
            {
                jsonResult = JsonConvert.DeserializeObject<ArtistWrapperDto>(response);
                return jsonResult.result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<SnippetDto>> GetArtistSongs(string artistName)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _youtubeApiKey,
                ApplicationName = this.GetType().ToString(),

            });
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = artistName;
            searchListRequest.MaxResults = 50;
            var searchListResponse = searchListRequest.Execute();

            List<SnippetDto> results = new List<SnippetDto>();

            foreach (var searchResult in searchListResponse.Items)
            {
                if (searchResult.Id.Kind.Contains("youtube#video") )
                {
                    results.Add( new SnippetDto()
                    {
                        id = searchResult.Id.VideoId,
                        title = searchResult.Snippet.Title,
                        thumbnail = searchResult.Snippet.Thumbnails,
                        kind = YoutubeEnums.video
                    });
                }
            }

            return results;
        }

        public async Task RemoveFiles(string fileName)
        {
            File.Delete(GetDownloadDirctory() + $@"\{fileName}.mp4");
            File.Delete(GetDownloadDirctory() + $@"\{fileName}.mp3");
            File.Delete(GetDownloadDirctory() + $@"\{fileName}Cuted.mp3");
        }

        private string GetDownloadDirctory()
        {
            return _hostEnvironment.WebRootPath + @"\Downloads";
        }
    }
}