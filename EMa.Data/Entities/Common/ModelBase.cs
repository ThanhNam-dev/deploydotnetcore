using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMa.Data.Entities.Common
{
    public partial class ModelBase : IDateTracking
    {
        /// <summary>
        /// Id with K : Type
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Created UserId
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Modified UserId
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Delete status
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Deleted UserId
        /// </summary>
        public string DeletedBy { get; set; }
    }
}
