namespace StoreRopa.Models.Vo
{
    public class RolesVO
    {
        public RolesVO() { }
        public RolesVO(Roles roles) {
            this.roles = roles;
        }
        public Roles roles { get; set; } = null!;
    }
}
