using Research_Software_Dev.Models.Forms;
using System.ComponentModel.DataAnnotations;

public class FormQuestionOption
{
    [Key]
    public string OptionId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string OptionText { get; set; }

    public double? OptionValue { get; set; }

    [Required]
    public string FormQuestionId { get; set; }
    public FormQuestion? FormQuestion { get; set; }
}
