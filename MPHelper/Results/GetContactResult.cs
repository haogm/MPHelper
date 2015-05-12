using MPHelper.DTOs;

namespace MPHelper.Results
{
	internal class GetContactResult
	{
		public BaseResp base_resp { get; set; }

		public ContactInfo contact_info { get; set; }
	}
}
