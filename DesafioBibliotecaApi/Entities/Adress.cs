using DesafioBibliotecaApi.Entidades;
using Newtonsoft.Json;
using System;

namespace DesafioBibliotecaApi.Entities
{
    public class Adress : BaseEntity<Guid>
    {
        public Adress(string zipCode, string street, string complement, string district, string location, string state, Client client)
        {
            ZipCode = zipCode;
            Street = street;
            Complement = complement;
            District = district;
            Location = location;
            State = state;
            Client = client;
            Id = Guid.NewGuid();    
        }

        [JsonProperty("cep")]
        public string ZipCode { get; set; }
        [JsonProperty("logradouro")]
        public string Street { get; set; }
        [JsonProperty("complemento")]
        public string Complement { get; set; }
        [JsonProperty("bairro")]
        public string District { get; set; }
        [JsonProperty("localidade")]
        public string Location { get; set; }
        [JsonProperty("uf")]
        public string State { get; set; }
        public Client Client { get; set; }
        

        public void Valida()
        {
            if (string.IsNullOrEmpty(Street) || Street.Length > 150)
                throw new Exception("Invalid Street");

            if (!string.IsNullOrEmpty(Complement) && Complement.Length > 100)
                throw new Exception("Invalid complement");

            if (string.IsNullOrEmpty(District) || District.Length > 100)
                throw new Exception("Invalid district");

            if (!string.IsNullOrEmpty(Location) && Location.Length > 100)
                throw new Exception("Invalid location");

            if (string.IsNullOrEmpty(State) || State.Length > 2)
                throw new Exception("Invalid state");

        }
    }
}
