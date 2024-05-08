using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace bookAppApi.Model
{
    [XmlRoot(ElementName = "User")]
    public class User
    {
        [Key]
        [XmlElement(ElementName = "id")]
        public int id { get; set; }

        [Required]
        [XmlElement(ElementName = "username")]
        public string username { get; set; }

        [Required]
        [XmlElement(ElementName = "password")]
        public string password { get; set; }

        [Required]
        [XmlElement(ElementName = "level")]
        public string level { get; set; }

        [XmlElement(ElementName = "image")]
        public string image { get; set; }


    }
}
