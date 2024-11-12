using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIA.Models
{
    public class Visita
    {
        public string Id { get; set; }  // ID único para cada visita
        public string Residente { get; set; }
        public string VisitorName { get; set; }
        public string VisitType { get; set; }
        public string QRCode { get; set; }  // El código QR generado
        public bool AccesoAutorizado { get; set; }
        public string ExpirationDate { get; set; }
        public string DateCreated { get; set; }
    }
}
