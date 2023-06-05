using System.ComponentModel.DataAnnotations;
 
namespace FacilitiesRequisition.Models.FacilityRequests; 

public enum Venues {
    [Display(Name = "The Struggle Square")]
    TheStruggleSquare = 1,
    
    [Display(Name = "St. Cecilia Auditorium")]
    StCeciliaAuditorium = 2,
    
    [Display(Name = "Silungan")]
    Silungan = 3,
    
    [Display(Name = "Quadrangle")]
    Quadrangle = 4,
    
    [Display(Name = "Classrooms")]
    Classrooms = 5,
    
    [Display(Name = "Board Room")]
    BoardRoom = 6,
    
    [Display(Name = "Cabalen Hall")]
    CabalenHall = 7,
    
    [Display(Name = "St. John Paul Activity Room 1")]
    StJohnPaulActRoom1 = 8,
    
    [Display(Name = "St. John Paul Activity Room 2")]
    StJohnPaulActRoom2 = 9
}