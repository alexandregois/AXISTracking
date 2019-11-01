using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace axistracking.UITestMultiplataforma.Tests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
	public class ViewLoginTests
    {
        IApp app;
        Platform platform;

		public ViewLoginTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }
        
        [Test]
        public void Login()
        {
			Func<AppQuery, AppQuery> queryWaitEmail = e => e.Marked("TxtEmail");
			Func<AppQuery, AppQuery> queryWaitSenha = e => e.Marked("TxtSenha");
			Func<AppQuery, AppQuery> queryWaitBtn = e => e.Marked("BtnLogin");
			Func<AppQuery, AppQuery> queryWaitDash = e => e.Marked("ListPainel");

			app.WaitForElement(queryWaitEmail, "Campo email não criado");
			app.WaitForElement(queryWaitSenha, "Campo senha não criado");
			app.WaitForElement(queryWaitBtn, "Botão Login não criado");

			app.EnterText(queryWaitEmail, "abc@gmail.com");
			AppInitializer.CloseKeyboard(this.platform, app);

			app.EnterText(queryWaitSenha, "12345");
			AppInitializer.CloseKeyboard(this.platform, app);


			app.Tap(queryWaitBtn);

			try
			{
				app.WaitForElement(queryWaitDash
				                   , "Não chegou na página de dashboard");
				Assert.True(true);
			}
			catch(Exception ex)
			{
				Assert.Null(ex, ex.Message);
			}

        }

        [Test]
        public void ServiceLoginNull()
        {

        }
       
    }
}

