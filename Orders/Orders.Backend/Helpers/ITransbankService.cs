using Orders.Shared.DTOs;

namespace Orders.Backend.Helpers
{
    public interface ITransbankService
    {
        TransbankResponseGlobal CreateTransaccion(TransbankRequestDTO transbankrequest);

        TransbankResponseGlobal ConfirmarTransaccion(string token);

        TransbankResponseGlobal ObtenerEstadoTransaccion(string token);

        TransbankResponseGlobal ReversarAnularTransaccion(TransbankReversarAnular request);

    }
}
