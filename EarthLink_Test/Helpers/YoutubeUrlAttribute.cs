using System.ComponentModel.DataAnnotations;

namespace EarthLink_Test.Helpers
{
    public class YoutubeUrlAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            if (value.ToString()?.Contains("youtube.com") == true)
                return true;
            return false;
        }
    }
}