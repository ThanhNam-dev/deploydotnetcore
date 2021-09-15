using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMa.API.Entity
{
	public class UserConfirm
	{
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		public int Code { get; set; }
	}
}
