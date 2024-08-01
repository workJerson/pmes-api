using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class AccessCodeProfile
{
    public int ProjectId { get; set; }

    public int AccessCodeProfileId { get; set; }

    public string AccessCodeProfileName { get; set; } = null!;

    public string AccessCodeProfileDescription { get; set; } = null!;

    public bool IsHidden { get; set; }

    public Guid Guid { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<AccessCodeProfilePermission> AccessCodeProfilePermissions { get; set; } = new List<AccessCodeProfilePermission>();

    public virtual ICollection<AccountAccesscodeProfile> AccountAccesscodeProfiles { get; set; } = new List<AccountAccesscodeProfile>();

    public virtual Project Project { get; set; } = null!;
}
