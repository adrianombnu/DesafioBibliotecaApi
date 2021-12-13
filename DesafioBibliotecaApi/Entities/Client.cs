using DesafioBibliotecaApi.Entidades;
using System;

namespace DesafioBibliotecaApi.Entities
{
    public class Client : Base
    {
        public string Name { get; set; }
        public string Lastname { get; set; } 
        public string Document { get; set; } 
        public int Age { get; set; }
        public string ZipCode { get; set; }
        public DateTime Birthdate { get; set; }
        public Guid IdUser { get; set; }
        public Adress Adress { get; set; } 

        public void Update(Client client)
        {
            Name = client.Name; 
            Lastname = client.Lastname; 
            Document = client.Document; 
            Age = client.Age;   
            ZipCode = client.ZipCode;   
            Adress = client.Adress;
        }
    }
}
