using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;
using Xamarin.Forms;

namespace axistracking.Domain.Dto
{
	public class PainelDto //: RealmObject
	{
		[PrimaryKey]
		public int Id { get; set; }
        public Int32 Total { get; set; }
        public String Chave { get; set; }
        public Boolean HasDetalhes { get; set; }
		public DateTimeOffset LastSearch { get; set; }

		[Ignored]
		public List<GraficoDto> Grafico { get; set; }
    }
}
