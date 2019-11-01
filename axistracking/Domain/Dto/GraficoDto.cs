using System;
using Realms;

namespace axistracking.Domain.Dto
{
	public class GraficoDto: RealmObject
	{
		public String Identificacao { get; set; }
		public double porcento { get; set; }
		public string corBarra { get; set; }
		public string corFundo { get; set; }
		public Byte Status { get; set; }
		public Int32 parent { get; set; }
    }
}
