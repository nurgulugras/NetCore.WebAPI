using System;
using Elsa.NNF.Data.ORM;

namespace ALMS.Model
{
    public interface IEntityBase : IEntity<int>
    {
        DateTime CreateDate { get; set; }
    }
}