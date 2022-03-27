using System.ComponentModel.DataAnnotations;

namespace Lista13.Models
{
    public class OrderCredentials
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Surname { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(30)]
        public string PaymentMethod { get; set; }
    }
}
