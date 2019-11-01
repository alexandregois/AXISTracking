using System;
using System.Threading;
using System.Threading.Tasks;
using axistracking.Domain;
using axistracking.Domain.Dto;
using axistracking.Domain.Realm;
using axistracking.Services.ServiceRealm;

namespace axistracking.Model
{
    public class ModelLogin : ModelBase
    {

        public async Task<ServiceResult<Token>> Login(
            string paramUsuario
            , string paramSenha
            , string paramHash
            , string paramIdAplicacao
            , string paramIdentificacao
            , string paramIdSistemaOperacional
            , CancellationToken paramToken
        )
        {
            ServiceResult<Token> result = new ServiceResult<Token>();
            try
            {

                result = await DataStore.Login(
                    paramUsuario
                    , paramSenha
                    , paramHash
                    , paramIdAplicacao
                    , paramIdentificacao
                    , paramIdSistemaOperacional
                    , paramToken
                );

            }
            catch (Exception)
            {
                result.MessageError = "ErroInexperado";
            }

            return result;
        }

        public async Task Deslogar(CancellationToken paramToken)
        {
            try
            {
                DataStore.Logoff(
                    _app.Token.IdAplicativo
                    , paramToken
                );
            }
            catch (Exception ex)
            { }
        }

        public async Task Atualiza_PushKey(
            String paramPushKey
            , CancellationToken paramToken
        )
        {
            try
            {

                TokenDataStore store = new TokenDataStore();
                TokenRealm _token = store.Get(1);


                ServiceResult<Boolean> result = await DataStore.Atualiza_PushKey(
                    _token.IdAplicativo //_app.Token.Aplicativo.IdAplicativo
                    , paramPushKey
                    , paramToken
                );
            }
            catch (Exception) { }
        }
    }
}
