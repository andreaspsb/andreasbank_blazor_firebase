using System;

namespace myapp.Models
{
    public class TransferenciaRequest
    {
        public int ContaOrigemId { get; set; }
        public int ContaDestinoId { get; set; }
        public decimal Valor { get; set; }
    }
}