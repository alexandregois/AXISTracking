using axistracking.Domain.Dto;
using axistracking.Domain.Realm;
using family.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axistracking.Domain
{
    public class Token
    {
        public String Access_token { get; set; }
        public String UrlLogo { get; set; }
        public List<PainelDto> LstDashBoard { get; set; }
        public AplicativoDto Aplicativo { get; set; }
        public String LstFuncao { get; set; }
        public String NomeCliente { get; set; }
        public String NomeUsuario { get; set; }


        #region Aplicativo
        public Int32 IdAplicativo { get; set; }
        #endregion


        public void TransformFromRealm(TokenRealm paramToken)
        {
            Access_token = paramToken.Access_Token;          
            LstFuncao = paramToken.LstFuncao;
            Aplicativo = new AplicativoDto();
            Aplicativo.IdAplicativo = paramToken.IdAplicativo;
            UrlLogo = paramToken.UrlLogo;

            IdAplicativo = paramToken.IdAplicativo;
            UrlLogo = paramToken.UrlLogo;
        }

    }
}
