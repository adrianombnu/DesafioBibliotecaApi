using DesafioBibliotecaApi.Entidades;
using Newtonsoft.Json;
using System;

namespace DesafioBibliotecaApi.Entities
{
    public class Adress : BaseEntity<Guid>
    {
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
    }
}
