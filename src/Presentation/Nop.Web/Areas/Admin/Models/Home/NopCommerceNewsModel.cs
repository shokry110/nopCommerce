using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Home;

/// <summary>
/// Represents a sarayetel news model
/// </summary>
public partial record sarayetelNewsModel : BaseNopModel
{
    #region Ctor

    public sarayetelNewsModel()
    {
        Items = new List<sarayetelNewsDetailsModel>();
    }

    #endregion

    #region Properties

    public List<sarayetelNewsDetailsModel> Items { get; set; }

    public bool HasNewItems { get; set; }

    public bool HideAdvertisements { get; set; }

    #endregion
}