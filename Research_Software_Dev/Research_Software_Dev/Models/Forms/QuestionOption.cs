using Research_Software_Dev.Models.Forms;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class QuestionOption
{
    [Key]
    public string OptionId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string OptionText { get; set; }

    public double? OptionValue { get; set; }

    // Foreign key to FormQuestion
    [Required]
    public string FormQuestionId { get; set; }
    [ForeignKey("FormQuestionId")]
    public FormQuestion FormQuestion { get; set; }
}
