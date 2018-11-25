using BusSimulator.Core.Data;
using BusSimulator.Core.Data.Interfaces;
using BusSimulator.Core.DataAccess.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BusSimulator.Core.DataAccess
{
    public abstract class BaseDataAccess<T> : IDataAccess<T> where T : RepositoryObject
    {
        private readonly Func<IDataRepository, List<T>> slotFunc;
        private readonly IDataRepository repository;

        protected BaseDataAccess(IDataRepository repository, Func<IDataRepository, List<T>> slotFunc)
        {
            this.repository = repository;
            this.slotFunc = slotFunc;
        }

        public void Add(T obj)
        {
            Debug.WriteLine($"[Data] Create new {typeof(T).Name}");

            var list = this.GetSlot();
            obj.Id = this.GetNextId(list);

            Debug.WriteLine($"[Data] New id for {typeof(T).Name}");

            list.Add(obj);
            this.repository.Save();
        }

        public void Delete(T obj)
        {
            Debug.WriteLine($"[Data] Deleting {typeof(T).Name} {obj.Id}");

            var list = this.GetSlot();
            list.RemoveAll(o => o.Id == obj.Id);
        }

        public List<T> GetAll()
        {
            Debug.WriteLine($"[Data] Getting all {typeof(T).Name}");

            return this.GetSlot()
                .Select(o => this.ResolveComplexTypes(o))
                .ToList();
        }

        public T GetById(int id)
        {
            Debug.WriteLine($"[Data] Getting {typeof(T).Name} {id}");

            return this.GetSlot()
                 .Where(o => o.Id == id)
                 .Select(o => this.ResolveComplexTypes(o))
                 .FirstOrDefault();
        }

        public void Update(T obj)
        {
            Debug.WriteLine($"[Data] Updating {typeof(T).Name} {obj.Id}");
            var oldObj = this.GetById(obj.Id);

            if (oldObj != null)
            {
                var slot = this.GetSlot();
                var indexOfOld = slot.IndexOf(oldObj);

                if (indexOfOld != -1)
                {
                    this.ResolveComplexTypes(obj).CopyTo(slot[indexOfOld]);
                }
            }
        }

        protected abstract T ResolveComplexTypes(T obj);

        private List<T> GetSlot()
        {
            return this.slotFunc(this.repository);
        }

        private int GetNextId(List<T> objects)
        {
            checked
            {
                if (objects.Count > 0)
                {
                    return objects.Max(o => o.Id) + 1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}
