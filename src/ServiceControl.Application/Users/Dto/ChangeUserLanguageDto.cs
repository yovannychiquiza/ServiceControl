using System.ComponentModel.DataAnnotations;

namespace ServiceControl.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}