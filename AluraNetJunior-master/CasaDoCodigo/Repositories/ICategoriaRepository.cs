using CasaDoCodigo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface ICategoriaRepository
    {
        Task SaveCategorias(List<Categoria> categorias);
        Task SaveCategorias(List<Livro> livros);
        Task SaveCategoria(string categoria);
        IList<Categoria> GetCategorias();
        Categoria GetCategoria(string categoria);
        Categoria GetCategoria(int id);
    }
}