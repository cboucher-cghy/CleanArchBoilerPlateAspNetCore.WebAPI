using CleanArchBoilerPlateAspNetCore.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class Gate
    {
        public int Id { get; set; }

        public int? GateRootId { get; set; }

        public Gate GateRoot { get; set; }

        public int Index { get; set; }

        public int ParentIndex { get; set; }

        public NodeType NodeType { get; set; }

        [Required]
        public bool IsNot { get; set; }

        public int FeatureId { get; set; }

        public Feature Feature { get; set; }

        public int FeatureValueId { get; set; }

        public FeatureValue FeatureValue { get; set; }

        public Operator Operator { get; set; }

        [Required]
        public string InsertKey { get; set; }
    }
}
