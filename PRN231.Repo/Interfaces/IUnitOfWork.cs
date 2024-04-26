using PRN231.Repo.Models;
using PRN231.Repo.Repository;

namespace PRN231.Repo.Interfaces;

public interface IUnitOfWork
{
    GenericRepository<Student> StudentRepository { get; }
    GenericRepository<UserRole> UserRoleRepository { get; }
    GenericRepository<StudentGroup> StudentGroupRepository { get; }
    void Save();
}