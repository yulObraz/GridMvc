//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace GridMvc.Site.Models
{
	using System;
	using System.Collections.Generic;

	public enum ShipperEnum { // Generally bad decision to use enum for it.
		Unknown,
		[Display(Name = "Speedy Express")]
		SpeedyExpress,
		[Display(Name = "United Package")]
		UnitedPackage,
		[Display(Name = "Federal Shipping")]
		FederalShipping
	};

    
    public partial class Shipper
    {
        public Shipper()
        {
            this.Orders = new HashSet<Order>();
        }
        [Key]
        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    
        public virtual ICollection<Order> Orders { get; set; }
    }
}
