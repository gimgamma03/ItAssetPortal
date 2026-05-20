using System.ComponentModel.DataAnnotations;

namespace ItAssetPortal.Models;

public class Asset
{
    public int Id { get; set; }

    [Required(ErrorMessage = "자산 이름을 입력하세요.")]
    [StringLength(100)]
    [Display(Name = "자산 이름")]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    [Display(Name = "시리얼 번호")]
    public string? SerialNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}