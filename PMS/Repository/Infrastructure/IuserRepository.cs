using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IuserRepository : IGenericRepository<user_detail>
    {
        List<user_detail> SearchUser();
    }
}
