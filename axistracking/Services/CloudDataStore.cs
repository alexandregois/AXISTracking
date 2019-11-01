using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using axistracking.Domain;
using axistracking.Domain.Dto;
using Newtonsoft.Json;
using axistracking.Services.ServiceRealm;
using axistracking.Domain.Realm;
using System.Threading;
using axistracking.Resx;

namespace axistracking.Services
{
    public class CloudDataStore : IDataStore
    {
        //private App _app => (Application.Current as App);
        public HttpClient client { get; set; }
        public TokenDataStore _tokenDataStore { get; set; }

        public String mediaType { get; set; } = "application/x-www-form-urlencoded";

        public CloudDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(App.Configuracao.URLStringWS);
            _tokenDataStore = new TokenDataStore();
        }

        private void ConfiguraChamadaPadrao()
        {
            TokenRealm _token = _tokenDataStore.Get(1);
            if (_token != null)
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic",
                                                  _token.Access_Token);
            }

            client.DefaultRequestHeaders.IfModifiedSince = DateTime.Now;

        }

        public async Task<ServiceResult<AncoraAtivacaoDto>> AtivarAncora(
            PosicaoHistorico paramPosicao
            , CancellationToken paramToken
        )
        {

            List<KeyValuePair<string, string>> requisaoParametros =
                 new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdTracedUnit"
                    , paramPosicao.IdUnidadeRastreada.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLatitude"
                    , paramPosicao.Latitude.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLongitude"
                    , paramPosicao.Longitude.ToString()
                )
            );

            //requisaoParametros.Add(
            //    new KeyValuePair<string, string>(
            //        "tolerancia"
            //        , null
            //    )
            //);

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramTolerance"
                    , "300"
                )
            );

            FormUrlEncodedContent content =
                new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            client.DefaultRequestHeaders
                  .Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            String urlRequisicao;

            urlRequisicao = "Anchor";


            return await MakeRequisicion<AncoraAtivacaoDto>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );

        }

        public async Task<ServiceResult<Boolean>> ComandoCancelar(
           int paramIdCommandLog
            , CancellationToken paramToken
        )
        {

            client.DefaultRequestHeaders
                  .Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            String urlRequisicao;

            urlRequisicao = "Command?paramIdCommandLog=" + paramIdCommandLog.ToString();

            return await MakeRequisicion<Boolean>(
                HttpMethod.Put
                , urlRequisicao
                , null
                , paramToken
            );

        }

        public async Task<ServiceResult<Int32>> DesativarAncora(
            PosicaoHistorico paramPosicao
            , CancellationToken paramToken
        )
        {

            client.DefaultRequestHeaders
                  .Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            String urlRequisicao;

            urlRequisicao = "Anchor/" + paramPosicao.IdUnidadeRastreada.ToString();


            return await MakeRequisicion<Int32>(
                HttpMethod.Delete
                , urlRequisicao
                , null
                , paramToken
            );

        }

        public async Task<ServiceResult<bool?>> Logoff(
            Int32 paramIdAplicativo
            , CancellationToken paramToken
        )
        {
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramAplicativo"
                    , paramIdAplicativo.ToString()
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            String urlRequisicao = "Logoff";

            return await MakeRequisicion<Boolean?>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

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
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "username"
                    , paramUsuario
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "password"
                    , paramSenha
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "hash"
                    , paramHash
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "idaplicacao"
                    , "10"
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "identificacao"
                    , paramIdentificacao
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "idsistemaoperacional"
                    , paramIdSistemaOperacional
                )
            );



            FormUrlEncodedContent content =
                new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            client.DefaultRequestHeaders
                  .Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            String urlRequisicao = "login/AuthenticateUserTracking";

            return await MakeRequisicion<Token>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }

        #region Position
        public async Task<ServiceResult<Posicao>> GetPosition(
            long paramIdPosicao
            , Int32 paramUnidadeRatreada
            , Byte? paramOrdemRastreador
            , CancellationToken paramToken
        )
        {
            String urlRequisicao;

            urlRequisicao = "Position/" + paramIdPosicao
                + "?paramIsCompleta=true"
                + "&paramUnidadeRastreada=" + paramUnidadeRatreada
                + "&paramOrdem=" + paramOrdemRastreador
                ;

            return await MakeRequisicion<Posicao>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }
        #endregion

        public async Task<ServiceResult<List<PosicaoHistorico>>>
        ListTracedUnitsWithPosition(
            CancellationToken paramToken
        )
        {

            String urlRequisicao = "Position";

            return await MakeRequisicion<List<PosicaoHistorico>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<String>> RetornaObjetoKeepAlive(Byte paramIdObjetoKeepAlive,
            CancellationToken paramToken,
            int paramIdAplicativo
        )
        {
            String urlRequisicao = "Synchronism/?paramIdTipoObjeto=" + paramIdObjetoKeepAlive.ToString() + "&paramIdAplicativo=" + paramIdAplicativo.ToString();

            return await MakeRequisicion<String>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<List<PosicaoHistorico>>>
        ListTracedUnitsWithAlert(
            CancellationToken paramToken
        )
        {

            String urlRequisicao = "Alert";

            return await MakeRequisicion<List<PosicaoHistorico>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<List<PosicaoHistorico>>>
        ListTracedUnitHistory(
            Int32 paramId
            , String paramData
            , Int32 paramPeriodo
            , Byte? paramOrdemRastreador
            , CancellationToken paramToken
        )
        {
            String urlRequisicao;

            urlRequisicao = "PositionHistory/"
                + paramId + "?paramTipo=2" +
                "&paramPageSize=" + paramPeriodo +
                "&paramOrdem=" + paramOrdemRastreador
                ;


            if (!String.IsNullOrWhiteSpace(paramData))
            {
                urlRequisicao += "&paramInitialPeriod=" + paramData;
            }

            return await MakeRequisicion<List<PosicaoHistorico>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        #region Command
        public async Task<ServiceResult<List<ComandoLog>>> ListCommandLog(
            CancellationToken paramToken
        )
        {
            String urlRequisicao = "CommandLog";

            return await MakeRequisicion<List<ComandoLog>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<List<ComandoParametroDto>>> ListCommandParameter(
            int paramIdRastreador
            , int paramIdComando
            , CancellationToken paramToken
        )
        {
            String urlRequisicao = String.Format(
                "CommandParameter?paramIdRastreador={0}&paramIdComando={1}"
                , paramIdRastreador
                , paramIdComando
            );

            return await MakeRequisicion<List<ComandoParametroDto>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<List<ComandoDto>>> ListCommand(
            int paramIdRastreador
            , CancellationToken paramToken
        )
        {
            String urlRequisicao = String.Format(
                "Command?paramIdRastreador={0}"
                , paramIdRastreador
            );

            return await MakeRequisicion<List<ComandoDto>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        public async Task<ServiceResult<StatusComandoDto>> SendCommand(
            int paramIdTracker
            , int paramOrder
            , int paramIdCommand
            , string paramLstParametresCommand
            , CancellationToken paramToken
        )
        {
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdTracker"
                    , paramIdTracker.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramOrder"
                    , paramOrder.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramIdCommand"
                    , paramIdCommand.ToString()
                )
            );

            //requisaoParametros.Add(
            //	new KeyValuePair<string, string>(
            //		"paramIdRastreadorUnidadeRastreada"
            //		, paramIdRastreadorUnidadeRastreada.ToString()
            //	)
            //);

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "paramLstParametresCommand"
                    , paramLstParametresCommand
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            String urlRequisicao = "Command";

            return await MakeRequisicion<StatusComandoDto>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }
        #endregion

        public async Task<ServiceResult<List<PainelDto>>> ListPainel(
            CancellationToken paramToken
        )
        {
            String urlRequisicao = "Dashboard";

            return await MakeRequisicion<List<PainelDto>>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }



        public async Task<ServiceResult<HistoricoComandoRespost>> ListCommandHistory
        (
            int paramPage
            , int paramPageSize
            , CancellationToken paramToken
        )
        {
            String urlRequisicao = "CommandHistory?" +
                "paramPage=" + paramPage.ToString() + "&paramPageSize=" + paramPageSize.ToString();

            return await MakeRequisicion<HistoricoComandoRespost>(
                HttpMethod.Get
                , urlRequisicao
                , null
                , paramToken
            );
        }

        private async Task<ServiceResult<TEntity>> MakeRequisicion<TEntity>(
            HttpMethod paramMethod
            , String paramEndereco
            , HttpContent paramContent
            , CancellationToken paramToken
        )
        {
            DateTime dataInicio = DateTime.UtcNow;
            TimeSpan waitTime = TimeSpan.FromSeconds(1);

            ServiceResult<TEntity> result = new ServiceResult<TEntity>();
            RequestResult<TEntity> requestResult = new RequestResult<TEntity>();
            try
            {

                ConfiguraChamadaPadrao();

                String enderecoFinal = String.Format("/api/{0}", paramEndereco);

                HttpRequestMessage request = new HttpRequestMessage(
                    paramMethod
                    , enderecoFinal);

                if (paramContent != null)
                    request.Content = paramContent;

                HttpResponseMessage response = await client.SendAsync(
                    request
                    , paramToken
                );

                String resultJson = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    requestResult = JsonConvert.DeserializeObject<RequestResult<TEntity>>(resultJson);
                    if (requestResult.Result != null)
                    {
                        result = requestResult.Result;
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<ServiceResult<TEntity>>(resultJson);
                    }
                    //result = JsonConvert
                    //.DeserializeObject<ServiceResult<TEntity>>(resultJson);
                }
                else
                {
                    result.MessageError = response.StatusCode.ToString();
                }
            }
            catch (HttpRequestException ex)
            {
                result.MessageError = "HttpRequestException";
            }
            catch (Exception ex)
            {
                //result.MessageError = "StoreException";
                result.MessageError = AppResources.LoginSenhaInvalido;
            }

            int totalDemorado = (int)DateTime.UtcNow
                                             .Subtract(dataInicio)
                                             .TotalMilliseconds;
            Int32 tempo = (Int32)(waitTime.TotalMilliseconds - totalDemorado);

            if (tempo > 0)
            {
                await Task.Delay(tempo);
            }

            return result;
        }

        public async Task<ServiceResult<bool>> Atualiza_PushKey(
            int paramIdAplicativo
            , String paramPushKey
            , CancellationToken paramToken
        )
        {
            List<KeyValuePair<string, string>> requisaoParametros =
                new List<KeyValuePair<string, string>>();

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "idaplicativo"
                    , paramIdAplicativo.ToString()
                )
            );

            requisaoParametros.Add(
                new KeyValuePair<string, string>(
                    "pushkey"
                    , paramPushKey
                )
            );

            FormUrlEncodedContent content = new FormUrlEncodedContent(requisaoParametros);

            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            content.Headers.ContentType.CharSet = "UTF-8";

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            String urlRequisicao = "login/UpdatePushKey";

            return await MakeRequisicion<Boolean>(
                HttpMethod.Post
                , urlRequisicao
                , content
                , paramToken
            );
        }


    }
}
