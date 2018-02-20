using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Djm.OGame.Web.Api.Dal.Models;

namespace Djm.OGame.Web.Api.Dal
{
    public class OGameContext : DbContext
    {
        public OGameContext() : base("OGameDB")
        {
            Database.SetInitializer<OGameContext>(new DropCreateDatabaseIfModelChanges<OGameContext>());
        }

        public DbSet<Pin> Pins { get; set; }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry,IDictionary<object, object> items)
        {
            var result = base.ValidateEntity(entityEntry, items);
            CheckUniqueness(result);
            return result;
        }

        private void CheckUniqueness(DbEntityValidationResult result)
        {
            if (!(result.Entry.Entity is Pin pin)) return;

            if(Pins.Any(p => p.OwnerId == pin.OwnerId
                             && p.TargetId == pin.TargetId
                             && p.UniverseId == pin.UniverseId))

                result.ValidationErrors.Add(new DbValidationError("Uniqueness violated","This pin already exists"));
        }

        
    }
}
