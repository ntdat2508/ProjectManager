using ProjectManager.Entity;

namespace ProjectManager.Repository
{
    public interface IClasssRepository : IRepository<Classs>
    {
    }
    public class ClasssRepository : Repository<Classs>, IClasssRepository
    {
        public ClasssRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface IDepartmentRepository : IRepository<Department>
    {
    }
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface IProjectListRepository : IRepository<ProjectList>
    {
    }
    public class ProjectListRepository : Repository<ProjectList>, IProjectListRepository
    {
        public ProjectListRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
    public interface IInternRepository : IRepository<Intern>
    {
    }
    public class InternRepository : Repository<Intern>, IInternRepository
    {
        public InternRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface ISchoolYearRepository : IRepository<SchoolYear>
    {
    }
    public class SchoolYearRepository : Repository<SchoolYear>, ISchoolYearRepository
    {
        public SchoolYearRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface ISpecializedRepository : IRepository<Specialized>
    {
    }
    public class SpecializedRepository : Repository<Specialized>, ISpecializedRepository
    {
        public SpecializedRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface IStudentRepository : IRepository<Student>
    {
    }
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface ITeacherRepository : IRepository<Teacher>
    {
    }
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface ITrainingSystemRepository : IRepository<TrainingSystem>
    {
    }
    public class TrainingSystemRepository : Repository<TrainingSystem>, ITrainingSystemRepository
    {
        public TrainingSystemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface IUserManagementRepository : IRepository<UserManagement>
    {
    }
    public class UserManagementRepository : Repository<UserManagement>, IUserManagementRepository
    {
        public UserManagementRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
