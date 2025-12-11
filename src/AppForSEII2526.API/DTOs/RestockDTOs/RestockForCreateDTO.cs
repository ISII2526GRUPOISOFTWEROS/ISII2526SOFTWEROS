namespace AppForSEII2526.API.DTOs.RestockDTOs
{
    public class RestockForCreateDTO
    {
        //public ItemForCreateRestockDTO()
        //{
        //}

        public RestockForCreateDTO(/*int id,*/ string title, string deliveryAddress, string? description, DateTime? expectedDate, DateTime restockDate, decimal totalPrice, IList<RestockItemDTO> restockItems, string restockResponsible)
        {
           // Id = id;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            Description = description;
            ExpectedDate = expectedDate;
            RestockDate = restockDate;
            TotalPrice = totalPrice;
            RestockItems = restockItems;
            RestockResponsible = restockResponsible;
        }

        public RestockForCreateDTO()
        {
            RestockItems = new List<RestockItemDTO>();
            RestockResponsible = string.Empty;
        }



        //public int Id { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Title")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Title must have at least 2 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Introduce the item title")]
        public string Title { get; set; } = null!;

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Delivery address must have at least 5 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter your address for delivery")]
        public string DeliveryAddress { get; set; } = null!;

        public string? Description { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime RestockDate { get; set; }
        [Precision(5, 2)]
        public decimal TotalPrice { get; set; }

        //References
        public IList<RestockItemDTO> RestockItems { get; set; } 
        public string RestockResponsible { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RestockForCreateDTO dTO &&
                  // Id == dTO.Id &&
                   Title == dTO.Title &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   Description == dTO.Description &&
                   ExpectedDate == dTO.ExpectedDate &&
                   RestockDate == dTO.RestockDate &&
                   TotalPrice == dTO.TotalPrice &&
                   /*EqualityComparer<IList<RestockItemForCreateDTO>>.Default.Equals(RestockItems, dTO.RestockItems) */
                   RestockItems.SequenceEqual(dTO.RestockItems) &&
                   RestockResponsible == dTO.RestockResponsible;
        }
    }



}
