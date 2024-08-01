using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class StatusCategory
{
    public string StatusCategoryCode { get; set; } = null!;

    public string StatusCategoryName { get; set; } = null!;

    public Guid Guid { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<StatusPerCategory> StatusPerCategories { get; set; } = new List<StatusPerCategory>();
}
