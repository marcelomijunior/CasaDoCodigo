using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CasaDoCodigo.MVC.Areas.Catalogo.Models
{
    [DataContract]
    public class BaseModel : IComparable
    {
        [DataMember]
        public int Id { get; set; }

        public int CompareTo(object obj)
        {
            if (!(obj is BaseModel outro)) return 1;

            return Id.CompareTo(outro.Id);
        }

        public override bool Equals(object obj)
        {
            return obj is BaseModel model &&
                   Id == model.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
