using System.Collections.Generic;

namespace WX.Tools.Results
{
	internal class ModifyContactResult
	{
		public string ret { get; set; }
		public List<ModifyContactResultDetail> result { get; set; }
	}

	internal class ModifyContactResultDetail
	{
		public string fakeId { get; set; }
		public string ret { get; set; }
	}

}
