namespace Orders.Shared.Response
{
    public class ActionResponse<T>
    {
        public bool WassSuccees { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
    }
}