using System.Collections.Generic;

namespace Lista13.Models.ViewModel
{
    public class OrderViewModel
    {
        public OrderCredentials OrderCredentials { get; set; }
        public List<(Article article, int count)> Articles { get; set; }
    }
}
