using System.ComponentModel.DataAnnotations;

namespace Assessment.Models.ViewModel
{
    public class TweetAddRequestModel
    {
        [Required]
        public string Message { get; set; }
    }
}