namespace Backend.Dtos
{
    public class OrderDtoIn
    {
        public DateTime InputDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? BillCreatedDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int DocumentNr { get; set; }
        public double Brutto { get; set; }
        public double Netto { get; set; }
        public double Tax { get; set; }
        public int FK_CustomerId { get; set; }
        public int Bill { get; set; }
        public string PO { get; set; } = null!;
        public int FK_StatusId { get; set; }
        public string UID { get; set; } = null!;
    }
}
