using DesafioBibliotecaApi.Entidades;

namespace DesafioBibliotecaApi.DTOs
{
    public class BookFilterDTO : Base
    {
        public string Name { get; set; }
        public AuthorFilterDTO Author { get; set; }

        
    }
}
