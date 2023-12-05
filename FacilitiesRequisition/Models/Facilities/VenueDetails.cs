using System.ComponentModel.DataAnnotations;
using FacilitiesRequisition.Models.FacilityRequests;

namespace FacilitiesRequisition.Models.Facilities; 

public class VenueDetails {
    public Venues Venue { get; set; }
    public string VenueName => GetVenueDisplayName(Venue);
    public string VenueDetailExpanded => GetVenueSpecificDetails(Venue);

    public List<FacilityRequest> FacilityRequests { get; set; }

    private string GetVenueDisplayName(Venues venue) {
        var displayAttribute = typeof(Venues)
            .GetMember(venue.ToString())[0]
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .OfType<DisplayAttribute>()
            .FirstOrDefault();

        return displayAttribute?.Name ?? venue.ToString();
    }

    private string GetVenueSpecificDetails(Venues venue) {
        switch (venue) {
            case Venues.TheStruggleSquare:
                return "The Struggle Square is the symbol of pushing yourself to the limits, but it can also be a place where the students can make an event.";
            
            case Venues.StCeciliaAuditorium:
                return "St. Cecilia Auditorium is where different major events takes place.";
            
            case Venues.Classrooms:
                return "Air-conditioned classrooms are equipped with built-in flat monitors or LCD projectors, while discussion and lecture rooms have built-in LCD projectors and screens.";
            
            case Venues.Silungan:
                return "Silungan is a place where students can eat or make simple events for their college - making it a simple place with great memories.";
            
            case Venues.Quadrangle:
                return "Also known as Quad-A, this place holds numerous outside events just like the Intercollegiate Culturals and the like.";
            
            case Venues.BoardRoom:
                return "This is a place where students can conduct their meetings professionally.";
            
            case Venues.CabalenHall:
                return "Cabalen Hall is an extension of AUF Canteen, where people can eat in a buffet style or any events that requires food.";
            
            case Venues.StJohnPaulActRoom1:
                return "Just like the AUF Board Room, this place where students can invite professional people and make a conference or event here.";
            
            case Venues.StJohnPaulActRoom2:
                return "Just like the AUF Board Room, this place where students can invite professional people and make a conference or event here.";
            default:
                return "Default details";
        }
    }
}

