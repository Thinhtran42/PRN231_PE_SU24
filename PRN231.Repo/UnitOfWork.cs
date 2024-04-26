using PRN231.Repo.Interfaces;
using PRN231.Repo.Models;
using PRN231.Repo.Repository;

namespace PRN231.Repo;

  public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PRN231_SU23_StudentGroupDBContext _context;
        private GenericRepository<Student> studentRepository;
        private GenericRepository<UserRole> userRoleRepository;
        private GenericRepository<StudentGroup> studentGroupRepository;

        public UnitOfWork(PRN231_SU23_StudentGroupDBContext context)
        {
            _context = context;
        }

        private bool disposed = false;

        public GenericRepository<Student> StudentRepository
        {
            get
            {
                if (studentRepository == null)
                {
                    studentRepository = new GenericRepository<Student>(_context);
                }
                return studentRepository;
            }
        }

        public GenericRepository<UserRole> UserRoleRepository
        {
            get
            {
                if (userRoleRepository == null)
                {
                    userRoleRepository = new GenericRepository<UserRole>(_context);
                }
                return userRoleRepository;
            }
        }

        public GenericRepository<StudentGroup> StudentGroupRepository
        {
            get
            {
                if (studentGroupRepository == null)
                {
                    studentGroupRepository = new GenericRepository<StudentGroup>(_context);
                }
                return studentGroupRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }