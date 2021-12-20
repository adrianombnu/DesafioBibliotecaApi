using DesafioBibliotecaApi.Entidades;
using System;
using System.Text.RegularExpressions;

namespace DesafioBibliotecaApi.Entities
{
    public class Client : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Lastname { get; set; } 
        public string Document { get; set; } 
        public int Age { get; set; }
        public string ZipCode { get; set; }
        public DateTime Birthdate { get; set; }
        public Guid IdUser { get; set; }
        public Adress Adress { get; set; }

        public Client(string name, string lastname, string document, int age, string zipCode, DateTime birthdate, Guid idUser)
        {
            Name = name;
            Lastname = lastname;
            Document = document;
            Age = age;
            ZipCode = zipCode;
            Birthdate = birthdate;
            IdUser = idUser;
            Id = Guid.NewGuid();
        }

        public Client(string name, string lastname, string document, int age, string zipCode, DateTime birthdate, Guid idUser, Guid idClient)
        {
            Name = name;
            Lastname = lastname;
            Document = document;
            Age = age;
            ZipCode = zipCode;
            Birthdate = birthdate;
            IdUser = idUser;
            Id = idClient;
        }

        public void Update(Client client)
        {
            Name = client.Name; 
            Lastname = client.Lastname; 
            Document = client.Document; 
            Age = client.Age;   
            ZipCode = client.ZipCode;   
            Adress = client.Adress;
            Birthdate = client.Birthdate;

            Valida();

        }

        public void Valida()
        {
            Regex rgx = new Regex(@"[^a-zA-Z\s]");

            if (string.IsNullOrEmpty(Name) || Name.Length > 50 || rgx.IsMatch(Name))
                throw new Exception("Invalid name.");

            if (string.IsNullOrEmpty(Lastname) || Lastname.Length > 50 || rgx.IsMatch(Lastname))
                throw new Exception("Invalid lastname.");

            rgx = new Regex("[^0-9]");

            if (rgx.IsMatch(Document))
                throw new Exception("Invalid document.");

            if (ZipCode is null || ZipCode.Length > 50 || rgx.IsMatch(ZipCode))
                throw new Exception("Invalid zipcode.");

            if (Age <= 0)
                throw new Exception("Invalid age.");

        } 
    }
}
