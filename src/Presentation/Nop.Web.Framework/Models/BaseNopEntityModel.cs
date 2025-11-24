
namespace Nop.Web.Framework.Models;

/// <summary>
/// Represents base sarayetel entity model
/// </summary>
public partial record BaseNopEntityModel : BaseNopModel
{
    /// <summary>
    /// Gets or sets model identifier
    /// </summary>
    public virtual int Id { get; set; }
}