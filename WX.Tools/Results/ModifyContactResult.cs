using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WX.Tools.Results
{
	public class ModifyContactResult
	{
		public string ret { get; set; }
		public List<ModifyContactResultDetail> result { get; set; }
	}

	public class ModifyContactResultDetail
	{
		public string fakeId { get; set; }
		public string ret { get; set; }
	}

}
