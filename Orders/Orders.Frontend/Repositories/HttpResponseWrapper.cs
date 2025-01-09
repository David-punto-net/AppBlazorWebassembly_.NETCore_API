namespace Orders.Frontend.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public T? Response { get; }
        public bool Error { get; }
        public HttpResponseMessage HttpResponseMessage { get; set; }


        public async Task<string> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode = HttpResponseMessage.StatusCode;

            if (statusCode == System.Net.HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado";
            }
            if (statusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            if (statusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return "Debe estar logueado para ejecutar esta operación";
            }
            if (statusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return "No tiene permiso paras ejecutar esta operación";
            }

            return "Ha ocurrido un error inesperado.";
        }

    }
}