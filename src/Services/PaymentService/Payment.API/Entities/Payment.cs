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
        public List<string> TransactionID { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string OrderID { get; set; }
    }
}
