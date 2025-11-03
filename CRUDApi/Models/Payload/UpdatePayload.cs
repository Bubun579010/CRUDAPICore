namespace CRUDApi.Models.Payload
{
    public class UpdatePayload
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
    }
}
