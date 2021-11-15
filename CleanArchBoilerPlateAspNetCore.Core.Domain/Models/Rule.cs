using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class Rule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GateWhenId { get; set; }

        [ForeignKey("GateWhenId")]
        public Gate GateWhen { get; set; }

        public int GateThenId { get; set; }

        [ForeignKey("GateThenId")]
        public Gate GateThen { get; set; }

        public RuleType RuleType { get; set; }

        public int MatrixId { get; set; }

        public Matrix Matrix { get; set; }
    }
}
