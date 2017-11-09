using System.ComponentModel.DataAnnotations;

namespace productservice.Model
{
    public class AdditionalInformation
    {
        public int additionalInformationId { get; set; }
        [MaxLength(50)]
        public string Information { get; set; }
        [MaxLength(50)]
        public string Value { get; set; }
    }
}
