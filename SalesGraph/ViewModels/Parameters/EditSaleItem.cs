using SalesGraph.Core.DataAccess.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SalesGraph.ViewModels.Parameters
{
    public class EditSaleItem : AddSaleItem
    {
        [Required]
        public Guid Id { get; set; }

        public override SaleItem ToEntity()
        {
            var entity = base.ToEntity();
            entity.SaleItemId = Id;

            return entity;
        }
    }
}
