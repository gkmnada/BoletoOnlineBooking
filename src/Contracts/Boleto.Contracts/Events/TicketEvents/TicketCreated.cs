﻿namespace Boleto.Contracts.Events.TicketEvents
{
    public class TicketCreated
    {
        public string TicketID { get; set; }
        public string MovieID { get; set; }
        public string CinemaID { get; set; }
        public string HallID { get; set; }
        public string SessionID { get; set; }
        public string SeatID { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string UserID { get; set; }
    }
}
