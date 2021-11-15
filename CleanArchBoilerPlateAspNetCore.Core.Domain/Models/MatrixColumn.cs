using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class MatrixColumn
    {
        public int Id { get; set; }

        public int IdMatrix { get; set; }

        public Matrix Matrix { get; set; }

        public int IdFeature { get; set; }

        public Feature Feature { get; set; }
    }
}
