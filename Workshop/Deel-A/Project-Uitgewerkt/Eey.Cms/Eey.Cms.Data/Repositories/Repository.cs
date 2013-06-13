namespace Eey.Cms.Data.Repositories {
    using System.Collections.Generic;
    using System.Data.Entity;

    using Eey.Cms.Data.Entities;

    public interface IRepository<TEntity>
        where TEntity : class, IHasIdentifier {
        /// <summary>
        ///   Gets the specified entity by identifier or null
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        TEntity GetById(int id);

        /// <summary>
        ///   Returns all the entities of type <typeparamref name="TEntity" /> from the database
        /// </summary>
        /// <returns> </returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Adds the specified entity to the database
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Deletes the specified entity from the database
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes the entity with the specified id from the database
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        ///   Saves changes to the underlying database context
        /// </summary>
        void SaveChanges();
    }

    /// <summary>
    ///   Represents a base class for repositories
    /// </summary>
    /// <typeparam name="TEntity"> </typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IHasIdentifier {
        private readonly DatabaseContext context;

        private readonly DbSet<TEntity> entities;

        protected DbSet<TEntity> Entities {
            get {
                return this.entities;
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Repository(DatabaseContext context) {
            this.context = context;
            this.entities = context.Set<TEntity>();
        }

        /// <summary>
        ///   Gets the specified entity by identifier or null
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        public TEntity GetById(int id) {
            return this.Entities.Find(id);
        }

        /// <summary>
        ///   Returns all the entities of type <typeparamref name="TEntity" /> from the database
        /// </summary>
        /// <returns> </returns>
        public IEnumerable<TEntity> GetAll() {
            return this.Entities;
        }

        /// <summary>
        /// Adds the specified entity to the database
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity) {
            this.entities.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the database
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity) {
            this.entities.Remove(entity);
        }

        /// <summary>
        /// Deletes the entity with the specified id from the database
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id) {
            TEntity entity = this.GetById(id);

            if (entity != null) {
                this.entities.Remove(entity);
            }
        }

        /// <summary>
        ///   Saves changes to the underlying database context
        /// </summary>
        public void SaveChanges() {
            this.context.SaveChanges();
        }
    }
}