

namespace AppForSEII2526.API.DTOs.RestockDTOs
{
    public class RestockDetailDTO : ItemForCreateRestockDTO
    {
       
        public RestockDetailDTO(int id, string title, string deliveryAddress, string? description, 
            DateTime? expectedDate, DateTime restockDate, decimal totalPrice, 
            IList<RestockItemForCreateDTO> restockItems, string restockResponsible, string adminSurname)

            : base(id, 
                  title, 
                  deliveryAddress, 
                  description, 
                  expectedDate, 
                  restockDate, 
                  totalPrice, 
                  restockItems, 
                  restockResponsible)
        {
            Id = id; 
            AdminSurname = adminSurname;

        }

        public int Id { get; set; }
        public string AdminSurname { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RestockDetailDTO dTO &&
                   base.Equals(obj) &&
                   Id == dTO.Id &&
                   AdminSurname == dTO.AdminSurname;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, AdminSurname);
        }


    }
}
