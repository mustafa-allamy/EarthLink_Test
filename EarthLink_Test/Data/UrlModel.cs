using System.ComponentModel.DataAnnotations;
using EarthLink_Test.Helpers;

namespace EarthLink_Test.Data
{
    public class UrlModel
    {
        [Required(ErrorMessage = "This Field Is Required")]
        [Url(ErrorMessage = "Input Should Be Url")]
        [YoutubeUrl(ErrorMessage = "Input Should Be Youtube Url")]
        public string YoutubeUrl { get; set; }
    }
}