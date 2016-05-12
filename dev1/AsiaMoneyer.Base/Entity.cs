using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiaMoneyer
{
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /*protected Entity()
        {
            throw new NotImplementedException();
        }

        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            throw new NotImplementedException();
        }
        //
        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            throw new NotImplementedException();
        }
        */
        // Summary:
        //     Unique identifier for this entity.
        [Key]
        public virtual TPrimaryKey Id { get; set; }
        /*
        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
        //
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Checks if this entity is transient (it has not an Id).
        //
        // Returns:
        //     True, if this entity is transient
        public virtual bool IsTransient()
        {
            throw new NotImplementedException();
        }
        //
        public override string ToString()
        {
            throw new NotImplementedException();
        }*/
    }
}
