using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axistracking.Domain.Realm
{
    public class TokenRealm: RealmObject
    {
		[PrimaryKey]
		public int Id { get; set; }
        public String Access_Token { get; set; }
		public String UrlLogo { get; set; }
		public String NomeUsuario { get; set; }
		public String NomeCliente { get; set; }
		public String LstFuncao { get; set; }

		public Int32 IdAplicativo { get; set; }
	}
}
