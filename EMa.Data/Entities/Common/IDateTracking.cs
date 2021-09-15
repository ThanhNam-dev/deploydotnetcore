using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMa.Data.Entities.Common
{
    public class IDateTracking
    {
        /// <summary>
        /// Created Date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Created Time
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Modified Date
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Modified Time
        /// </summary>
        public DateTime? ModifiedTime { get; set; }
    }
}
