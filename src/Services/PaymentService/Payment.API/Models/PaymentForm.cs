namespace Payment.API.Models
{
    public class PaymentForm
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string CVV { get; set; }
    }
}
