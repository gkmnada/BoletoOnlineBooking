using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Payment.API.Entities
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PaymentID { get; set; }
        public string ConversationID { get; set; }
        public string ProcessID { get; set; }
        public List<string> TransactionsID { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentType { get; set; }
        public string CardType { get; set; }
        public string BinNumber { get; set; }
        public string LastFourDigits { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string PaymentStatus { get; set; } = Enums.Payment.PaymentStatus.Success.ToString();
        public string OrderID { get; set; }
    }
}
