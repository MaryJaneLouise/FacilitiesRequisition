namespace FacilitiesRequisition.Models.FacilityRequests; 

public abstract class Signature {
    public abstract int Id { get; set; }
    public abstract User User { get; }
    public abstract bool IsSigned { get; set; }
}