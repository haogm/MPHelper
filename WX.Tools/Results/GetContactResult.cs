using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WX.Tools.Results
{
	using WX.Tools.DTOs;

	internal class GetContactResult
	{
		public BaseResp base_resp { get; set; }

		public ContactInfo contact_info { get; set; }
	}
}
