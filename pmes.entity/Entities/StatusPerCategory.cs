using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class StatusPerCategory
{
    public string StatusCategoryCode { get; set; } = null!;

    public string StatusCode { get; set; } = null!;

    public string StatusPerCategoryCode { get; set; } = null!;

    public Guid Guid { get; set; }

    public int? SequenceNo { get; set; }

    public string? Remarks { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual StatusCategory StatusCategoryCodeNavigation { get; set; } = null!;

    public virtual Status StatusCodeNavigation { get; set; } = null!;
}
