namespace StoreRopa.Models.Vo
{
    public class PersonaClienteVO
    {
        public PersonaClienteVO()
        {
        }
        public PersonaClienteVO(Persona persona, Cliente cliente) { 
            this.persona = persona;
            this.cliente = cliente;
        }
        public Persona persona { get; set; } = null!;
        public Cliente cliente { get; set; } = null!;
    }
}
