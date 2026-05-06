using System;
using System.Collections.Generic;
using System.Text;

using TuPenca.Domain.Enums;

namespace TuPenca.Application.DTOs.Penca
{
    public class PencaEditPremioDto
    {
        public int PorcentajePremio1 { get; set; }
        public int PorcentajePremio2 { get; set; }
        public int PorcentajePremio3 { get; set; }
    }
}