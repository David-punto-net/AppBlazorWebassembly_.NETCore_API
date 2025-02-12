using Orders.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.DTOs
{
    public class CategoriaDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<CardDTO> Productos { get; set; } = new List<CardDTO>();
    }
}
