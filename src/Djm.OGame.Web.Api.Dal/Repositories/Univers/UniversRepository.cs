using Djm.OGame.Web.Api.Dal.Repositories.Base;
using Djm.OGame.Web.Api.Dal.Services;

namespace Djm.OGame.Web.Api.Dal.Repositories.Univers
{
    public class UniversRepository : Repository<Entities.Univers, int>, IUniversRepository
    {
        public UniversRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}