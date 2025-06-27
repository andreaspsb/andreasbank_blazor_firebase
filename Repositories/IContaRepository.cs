using myapp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myapp.Repositories
{
    public interface IContaRepository
    {
        Task<IEnumerable<Conta>> GetAll();
        Task<Conta> GetById(int id);
        Task Add(Conta conta);
        Task Update(Conta conta);
        Task Delete(int id);
    }
}