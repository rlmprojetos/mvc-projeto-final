using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        //public DbSet<Categoria> Categorias { get; set; }

        public CategoriaRepository(ApplicationContext context)
            : base(context)
        {

        }

        public IList<Categoria> GetCategorias()
        {
            return dbSet.ToList();
        }

        public Categoria GetCategoria(int id)
        {
            return dbSet.Where(c => c.Id == id).SingleOrDefault();
        }

        public Categoria GetCategoria(string nomeDaCategoria)
        {
            return dbSet.Where(c => c.NomeDaCategoria == nomeDaCategoria).SingleOrDefault();
        }

        public async Task SaveCategorias(List<Categoria> categorias)
        {
            foreach (var categoria in categorias)
            {
                if (!dbSet.Where(p => p.Id == categoria.Id).Any())
                {
                    dbSet.Add(new Categoria(categoria.NomeDaCategoria));
                }
            }
            await contexto.SaveChangesAsync();
        }

        public async Task SaveCategorias(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                if (dbSet.SingleOrDefault(c => c.NomeDaCategoria == livro.Categoria) == null)
                {
                    dbSet.Add(new Categoria(livro.Categoria));
                    await contexto.SaveChangesAsync();
                }
            }
        }

        public async Task SaveCategoria(string categoria)
        {
            var categori = contexto.Set<Categoria>()
                           .Where(p => p.NomeDaCategoria == categoria)
                           .SingleOrDefault();

            if (categori == null)
            {
                dbSet.Add(new Categoria(categoria));
                await contexto.SaveChangesAsync();
            }
        }
    }

}
