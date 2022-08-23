using System.Text.Json.Serialization;

namespace HPlusSport2.Api.Models
{
    public class Product
    {
        public int Id
        {
            get; set;
        }

        public string Sku
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
        
        public decimal Price
        {
            get; set;
        }
        public bool IsAvailable
        {
            get; set;
        }

        public int CategoryId
        {
            get; set;
        }
        [JsonIgnore]  // this prevents circular logic/infinite loop with product <> category <> product ...
        public virtual Category Category
        {
            get; set;
        }
    }
}
