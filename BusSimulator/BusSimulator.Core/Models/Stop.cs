using BusSimulator.Core.Data;

namespace BusSimulator.Core.Models
{
    public class Stop : RepositoryObject
    {
        public string Name { get; set; }

        public bool IsMajor { get; set; }

        public override void CopyTo(RepositoryObject other)
        {
            if (other is Stop otherStop)
            {
                otherStop.IsMajor = this.IsMajor;
                otherStop.Name = this.Name;
            }
        }
    }
}
