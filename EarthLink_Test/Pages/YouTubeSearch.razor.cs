using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using EarthLink_Test.Data;
using EarthLink_Test.Dtos;

namespace EarthLink_Test.Pages
{
    public partial class YouTubeSearch
    {
        private bool _spinnerDisplayState = false;
        private UrlModel _url = new UrlModel();
        private string _loadingText = "";
        private List<SnippetDto> _artistList = new List<SnippetDto>();
        private bool _errorState = false;
        private string _errorText = "";

        private async Task StartSearch()
        {
            _artistList.Clear();
            _spinnerDisplayState = true;
            _loadingText = "Downloading video Please Wait...";
            await Task.Delay(1);

            var fileName = Path.GetRandomFileName();

            var videoDownloadState = await YoutubeService.DownloadVideo(_url.YoutubeUrl, fileName);
            if (videoDownloadState != true)
            {
                _errorText = "Field to download";
                _errorState = true;
                _spinnerDisplayState = false;
                StateHasChanged();
                await Task.Delay(1);
            }

            _loadingText = "converting video to mp3";
            StateHasChanged();
            await Task.Delay(1);

            await YoutubeService.ConvertToMp3(fileName);
            //brake 5 sec to make sure video is saved 
            System.Threading.Thread.Sleep(5000);

            await YoutubeService.Cut1Min(fileName);

            _loadingText = "Recognizing Song";
            StateHasChanged();
            await Task.Delay(1);
            //brake 5 sec to make sure audio is saved 
            System.Threading.Thread.Sleep(5000);

            var artist = YoutubeService.RecognizeSong($"{fileName}Cuted.mp3").Result;
            if (artist == null)
            {
                _errorText = "Field to recognize, Are You sure it's a song?";
                _errorState = true;
                _spinnerDisplayState = false;
                StateHasChanged();
                await Task.Delay(1);
            }
            else
            {
                _loadingText = "Fetching artist list";
                StateHasChanged();
                await Task.Delay(1);
                _artistList = await YoutubeService.GetArtistSongs("The Chainsmokers");
            }


            _spinnerDisplayState = false;
            StateHasChanged();
            await Task.Delay(1);
            await YoutubeService.RemoveFiles(fileName);

        }

    }
}
