using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.DTOs;

public class TransbankResponseGlobal
{
    public bool Exito {  get; set; }
    public string? Mensaje { get; set; }
    public object? Data { get; set; }
}
