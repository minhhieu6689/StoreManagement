using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreManagement.Data.Model;
using StoreManangement.Data.Data;
using StoreManangement.Service.Repository;

namespace StoreManangement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IDataRepository<Employee> _employeeRepository;

        public EmployeesController(IDataRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            IEnumerable<Employee> employees = await _employeeRepository.GetAll();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,DateOfBirth,PhoneNumber,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,DateOfBirth,PhoneNumber,Email")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeRepository.Update(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.Get(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            var employee = _employeeRepository.Get(id);
            if(employee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }
    }
}
