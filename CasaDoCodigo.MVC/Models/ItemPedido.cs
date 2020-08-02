using CasaDoCodigo.MVC.Areas.Catalogo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CasaDoCodigo.MVC.Models
{
    [DataContract]
    public class ItemPedido : BaseModel
    {
        [Required]
        [DataMember]
        public Pedido Pedido { get; private set; }
        [Required]
        [DataMember]
        public string CodigoProduto { get; private set; }
        [Required]
        [DataMember]
        public string NomeProduto { get; private set; }
        [Required]
        [DataMember]
        public int Quantidade { get; private set; }
        [Required]
        [DataMember]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoUnitario { get; private set; }
        [DataMember]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal => Quantidade * PrecoUnitario;

        public ItemPedido()
        {

        }

        public ItemPedido(Pedido pedido, string codigoProduto, string nomeProduto, int quantidade, decimal precoUnitario)
        {
            Pedido = pedido;
            CodigoProduto = codigoProduto;
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        internal void AtualizaQuantidade(int quantidade)
        {
            Quantidade = quantidade;
        }
    }
}
