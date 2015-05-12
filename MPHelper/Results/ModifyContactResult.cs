using System.Collections.Generic;

namespace MPHelper.Results
{
	internal class ModifyContactResult
	{
		public int ret { get; set; }

		public List<ModifyContactResultDetail> result { get; set; }
	}

	internal class ModifyContactResultDetail
	{
		public string fakeId { get; set; }

		public string ret { get; set; }
	}
}
