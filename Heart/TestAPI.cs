using System;
using System.Web.Services;

namespace Heart
{
	
	[WebService (Namespace = "localhost")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class TestAPI : WebService
	{
		public TestAPI ()
		{
		}

		[WebMethod]
		public string Test()
		{
			return "Hello World!";
		}
	}
}

