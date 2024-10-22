namespace StoreRopa.Models
{
    public class User
    {
        public Int32 Id {  get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; } = "";
        public string RolName { get; set; }
        public Int32 RolId { get; set; }
        public Int32 IdPersona { get; set; }

    }
}
