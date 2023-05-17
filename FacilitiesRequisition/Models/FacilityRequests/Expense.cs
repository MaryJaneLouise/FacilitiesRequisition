namespace FacilitiesRequisition.Models.FacilityRequests; 

public class Expense {
    public int Id { get; set; }
    public FacilityRequest FacilityRequest { get; set; }
    public string Purpose { get; set; }
    public decimal AmountToPay { get; set; }
}