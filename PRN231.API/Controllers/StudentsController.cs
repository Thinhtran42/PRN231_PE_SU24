using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231.Repo.Interfaces;
using PRN231.Repo.Models;
using PRN231.Repo.ViewModels;

namespace PRN231.API.Controllers;

public class StudentsController : BaseController
{
    public readonly IUnitOfWork _unitOfWork;

    public readonly IMapper _mapper;

    public StudentsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    // GET api/student
    [Authorize(Roles = "staff")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents(int? groupId, int? minBirthYear, int? maxBirthYear, int? pageSize, int? pageIndex)
    {
        // Xây dựng biểu thức lambda để lọc dữ liệu
        Expression<Func<Student, bool>> filter = s => true; // Mặc định không áp dụng bất kỳ điều kiện nào

        if (groupId.HasValue)
        {
            filter = s => s.GroupId == groupId;
        }

        if (minBirthYear.HasValue && maxBirthYear.HasValue)
        {
            DateTime minDate = new DateTime(minBirthYear.Value, 1, 1);
            DateTime maxDate = new DateTime(maxBirthYear.Value, 12, 31);
            filter = s => s.DateOfBirth >= minDate && s.DateOfBirth <= maxDate;
        }

        var listStudent = await _unitOfWork.StudentRepository.GetAsync(
            filter,
            null, // Không sắp xếp theo thứ tự cụ thể
            "Group", // Bao gồm thông tin về nhóm của sinh viên
            pageIndex,
            pageSize
        );
        
        // Gọi phương thức GetAsync với các tham số tương ứng
        return Ok(listStudent);
    }

    [Authorize(Roles = "staff")]
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDetailViewModel>> GetStudentById(int id)
    {
        var studentById = await _unitOfWork.StudentRepository.GetByIDAsync(id);
        
        if (studentById == null)
        {
            return NotFound(); 
        }

        var groupOfStudent = await _unitOfWork.StudentGroupRepository.GetByIDAsync(studentById.GroupId);

        var studentViewModel = _mapper.Map<StudentDetailViewModel>(studentById);

        studentViewModel.GroupName = groupOfStudent.GroupName;

        return studentViewModel;
    }
    
    [Authorize(Roles = "staff")]
    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent(StudentViewModel createModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var student = _mapper.Map<Student>(createModel);
        _unitOfWork.StudentRepository.Insert(student);
        _unitOfWork.Save();
        
        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
    }
    
    [Authorize(Roles = "staff")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentViewModel updateViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var studentExisted = await _unitOfWork.StudentRepository.GetByIDAsync(id);
        if (studentExisted == null)
        {
            return NotFound();
        }
        _mapper.Map(updateViewModel, studentExisted);
        _unitOfWork.StudentRepository.Update(studentExisted);
        _unitOfWork.Save();
        return NoContent();
    }
    
    [Authorize(Roles = "staff")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (id <= 0) return BadRequest("Invalid Student Id");
        var studentById = await _unitOfWork.StudentRepository.GetByIDAsync(id);
        if (studentById == null)
        {
            return NotFound();
        }
        _unitOfWork.StudentRepository.Delete(studentById);
        _unitOfWork.Save();
        return NoContent();
    }
}