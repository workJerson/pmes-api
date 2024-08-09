using System;
using System.Collections.Generic;

namespace pmes.entity.Entities;

public partial class AccountInfo
{
    public string AccountTypeCode { get; set; } = null!;

    public string AccountTypeName { get; set; } = null!;

    public int AccountId { get; set; }

    public Guid Guid { get; set; }

    public string? AccountCode { get; set; }

    public string AccountName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string? WebsiteUrl { get; set; }

    public string? ImageUrl { get; set; }

    public string? CountryIsoCode2 { get; set; }

    public string? CountryName { get; set; }

    public string? RegionId { get; set; }

    public Guid? StateGuid { get; set; }

    public Guid? CityGuid { get; set; }

    public Guid? PostalGuid { get; set; }

    public string? RegionName { get; set; }

    public string? StateName { get; set; }

    public string? CityName { get; set; }

    public string? PostalCode { get; set; }

    public string? TownBaranggay { get; set; }

    public string? AddressLine { get; set; }

    public string? FullAddress { get; set; }

    public string? Longitude { get; set; }

    public string? Latitude { get; set; }

    public string? AddressAdditionalInformation { get; set; }

    public string? MapAddress { get; set; }

    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual ICollection<AccountAccessCodeProfile> AccountAccessCodeProfiles { get; set; } = new List<AccountAccessCodeProfile>();
}
