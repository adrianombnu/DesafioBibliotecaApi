using DesafioBibliotecaApi.Entidades;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Entities
{
    public class Author : Base
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Nacionality { get; set; }
        public int Age { get; set; }
        public List<Book>? Books { get; set; }

        public Author(string name, string lastname, string nacionality, int age)
        {
            Name = name;
            Lastname = lastname;
            Nacionality = nacionality;
            Age = age;
            
        }

        public void Update(Author author)
        {
            Name = author.Name; 
            Lastname = author.Lastname;
            Nacionality = author.Nacionality;
            Age = author.Age;

        }
                
    }
}
