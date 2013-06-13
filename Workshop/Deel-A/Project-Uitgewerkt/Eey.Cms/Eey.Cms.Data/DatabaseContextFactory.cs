namespace Eey.Cms.Data {
    using System.Data.Entity;

    using Eey.Cms.Data.Entities;

    public static class DatabaseContextFactory {
        public static DatabaseContext CreateDatabaseContext() {
            Database.SetInitializer(new DatabaseContextInitializer());    

            return new DatabaseContext();
        }
    }

    internal sealed class DatabaseContextInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext> {
        /// <summary>
        /// A method that should be overridden to actually add data to the context for seeding.
        ///                 The default implementation does nothing.
        /// </summary>
        /// <param name="context">The context to seed. </param>
        protected override void Seed(DatabaseContext context) {
            // create initial page
            {
                CmsPage page1 = new CmsPage();
                page1.Title = "Some Page";
                page1.Body = "Bla bla bla - bla bla bla bla bla Bla bla bla - bla bla bla bla bla ";
                page1.Parent = null;

                context.CmsPages.Add(page1);

                // create two child pages 
                {
                    CmsPage cpage1 = new CmsPage();
                    cpage1.Title = "Some Child Page";
                    cpage1.Body = "Bla bla2 bla Bla bla bla - bla bla bla bla bla Bla bla bla - bla bla bla bla bla ";
                    cpage1.Parent = page1;

                    context.CmsPages.Add(cpage1);
                }

                {
                    CmsPage cpage1 = new CmsPage();
                    cpage1.Title = "Some Page Child";
                    cpage1.Body = "Bla bla bla3 Bla bla bla - bla bla bla bla bla  Bla bla bla - bla bla bla bla bla ";
                    cpage1.Parent = page1;

                    context.CmsPages.Add(cpage1);
                }
            }

            context.SaveChanges();
        }
    }
}
