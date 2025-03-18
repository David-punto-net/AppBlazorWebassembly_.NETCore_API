using Orders.Shared.DTOs;
using Transbank.Common;
using Transbank.Webpay.Common;
using Transbank.Webpay.WebpayPlus;

namespace Orders.Backend.Helpers
{
    public class TransbankService : ITransbankService
    {
        private readonly Transaction _tx;
        private readonly TransbankResponseGlobal _response;

        public TransbankService()
        {
            _tx = new Transaction(new Options(IntegrationCommerceCodes.WEBPAY_PLUS, IntegrationApiKeys.WEBPAY, WebpayIntegrationType.Test));
            _response = new TransbankResponseGlobal();
        }

        public TransbankResponseGlobal CreateTransaccion(TransbankRequestDTO transbankrequest)
        {
            if (transbankrequest == null ||
               string.IsNullOrWhiteSpace(transbankrequest.Buy_order) ||
               string.IsNullOrWhiteSpace(transbankrequest.Session_id) ||
               string.IsNullOrWhiteSpace(transbankrequest.Return_url) ||
               transbankrequest.Amount <= 0)
            {
                _response.Exito = false;
                _response.Mensaje = "No se ingresaron datos validos";
                _response.Data = null;

                return _response;
            }

            try
            {
                var crearTransaccion = _tx.Create(transbankrequest.Buy_order, transbankrequest.Session_id, transbankrequest.Amount, transbankrequest.Return_url);

                var respuestaTransBank = new
                {
                    url = crearTransaccion.Url,
                    token = crearTransaccion.Token,
                    urlCompleta = crearTransaccion.Url + "?token_ws=" + crearTransaccion.Token
                };

                _response.Exito = true;
                _response.Mensaje = "Transacción generada con exito.";
                _response.Data = respuestaTransBank;

                return _response;

            }
            catch (Exception ex)
            {
                _response.Exito = false;
                _response.Mensaje = "Hubo un error: " + ex.Message;
                _response.Data = null;

                return _response;
            }

        }

        public TransbankResponseGlobal ConfirmarTransaccion(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _response.Exito = false;
                _response.Mensaje = "No se ingresaron datos validos";
                _response.Data = null;

                return _response;
            }

            try
            {
                var confirmarTransaccion = _tx.Commit(token);

                _response.Exito = true;
                _response.Mensaje = "Confirmación de la transacción.";
                _response.Data = confirmarTransaccion;

                return _response;

            }
            catch (Exception ex)
            {
                _response.Exito = false;
                _response.Mensaje = "Hubo un error: " + ex.Message;
                _response.Data = null;

                return _response;
            }
        }

        public TransbankResponseGlobal ObtenerEstadoTransaccion(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _response.Exito = false;
                _response.Mensaje = "No se ingresaron datos validos";
                _response.Data = null;

                return _response;
            }

            try
            {
                var obtenerEstadoTransaccion = _tx.Status(token);

                _response.Exito = true;
                _response.Mensaje = "Estado de la transacción.";
                _response.Data = obtenerEstadoTransaccion;

                return _response;

            }
            catch (Exception ex)
            {
                _response.Exito = false;
                _response.Mensaje = "Hubo un error: " + ex.Message;
                _response.Data = null;

                return _response;
            }
        }

        public TransbankResponseGlobal ReversarAnularTransaccion(TransbankReversarAnular request)
        {
            if (request == null ||
              string.IsNullOrWhiteSpace(request.Token) ||
              request.Amount <= 0)
            {
                _response.Exito = false;
                _response.Mensaje = "No se ingresaron datos validos";
                _response.Data = null;

                return _response;
            }

            try
            {
                var reversarAnularTransaccion = _tx.Refund(request.Token, request.Amount);

                _response.Exito = true;
                _response.Mensaje = "Transacción reversada o anulada";
                _response.Data = reversarAnularTransaccion;

                return _response;

            }
            catch (Exception ex)
            {
                _response.Exito = false;
                _response.Mensaje = "Hubo un error: " + ex.Message;
                _response.Data = null;

                return _response;
            }
        }

    }
}
