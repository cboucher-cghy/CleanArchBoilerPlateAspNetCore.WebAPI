using System.Collections.Generic;

namespace CleanArchBoilerPlateAspNetCore.Core.Domain.Models
{
    public class Matrix : AuditableEntity
    {
        public Matrix()
        {
            MatrixColumns = new List<MatrixColumn>();
            MatrixRows = new List<MatrixRow>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int IdContext { get; set; }

        public Context Context { get; set; }

        public List<MatrixColumn> MatrixColumns { get; set; }

        public List<MatrixRow> MatrixRows { get; set; }
    }
}
