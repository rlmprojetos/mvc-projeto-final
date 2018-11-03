using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;
        private readonly ICategoriaRepository categoriaRepository;

        public PedidoController(IProdutoRepository produtoRepository,
            IPedidoRepository pedidoRepository,
            IItemPedidoRepository itemPedidoRepository,
            ICategoriaRepository categoriaRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
            this.categoriaRepository = categoriaRepository;
        }

        public IActionResult Carrossel()
        {
            return View(produtoRepository.GetProdutos());
        }

        public async Task<IActionResult> Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                await pedidoRepository.AddItem(codigo);
            }

            Pedido taskPedido = await pedidoRepository.GetPedido();
            List<ItemPedido> itens = taskPedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return base.View(carrinhoViewModel);
        }

        public async Task<IActionResult> Cadastro()
        {
            var pedido = await pedidoRepository.GetPedido();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }

            return View(pedido.Cadastro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                return View(await pedidoRepository.UpdateCadastro(cadastro));
            }
            return RedirectToAction("Cadastro");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<UpdateQuantidadeResponse> UpdateQuantidade([FromBody]ItemPedido itemPedido)
        {
            return await pedidoRepository.UpdateQuantidade(itemPedido);
        }

        public async Task<IActionResult> BuscaDeProdutos()
        {
            var produtos = await produtoRepository.GetProdutos();
            for (int i = 0; i < produtos.Count; i++)
            {
                produtos[i].Categoria = categoriaRepository.GetCategoria(produtos[i].CategoriaId);
            }
            BuscaDeProdutosViewModel buscaDeProdutosViewModel = new BuscaDeProdutosViewModel(produtos);
            return base.View(buscaDeProdutosViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BuscaDeProdutos(string pesquisa)
        {
            var produtos = await produtoRepository.GetProdutos();
            for (int i = 0; i < produtos.Count; i++)
            {
                produtos[i].Categoria = categoriaRepository.GetCategoria(produtos[i].CategoriaId);
            }

            if (string.IsNullOrEmpty(pesquisa))
            {
                BuscaDeProdutosViewModel buscaDeProdutosViewModel = new BuscaDeProdutosViewModel(produtos);
                return base.View(buscaDeProdutosViewModel);
            }
            else
            {
                var resultadoBuscaProdutos = produtos.ToList().FindAll(a => a.Nome.Contains(pesquisa) ||
                                                                            a.Categoria.NomeDaCategoria.Contains(pesquisa));

                BuscaDeProdutosViewModel buscaDeProdutosViewModel = new BuscaDeProdutosViewModel(resultadoBuscaProdutos);

                if (resultadoBuscaProdutos == null || resultadoBuscaProdutos.Count == 0)
                {
                    TempData["Mensagem"] = "Nenhum produto encontrado";

                    return base.View(buscaDeProdutosViewModel);
                }
                else

                    return base.View(buscaDeProdutosViewModel);
            }
        }
    }
}
