using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Models
{
    public class AuthorForCreationDto
    {
        [Required(ErrorMessage = "必须提供姓名。")]
        [MaxLength(20, ErrorMessage = "姓名的最大长度为20个字符。")]
        public string Name { get; set; }


        public int Age { get; set; }

        [EmailAddress(ErrorMessage = "邮箱格式不正确。")]
        public string Email { get; set; }

    }
}
