namespace ALMS.Model
{
    public class AppProductDto2 : IDtoEntity
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}