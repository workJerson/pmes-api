using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class AccountAccessCodeProfile
{
    public int AccountId { get; set; }

    public int AccessCodeProfileId { get; set; }

    public int AccountAccessCodeProfileId { get; set; }

    public Guid Guid { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual AccessCodeProfile AccessCodeProfile { get; set; } = null!;

    public virtual AccountInfo Account { get; set; } = null!;
}
