using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheBugTracker.Data;
using TheBugTracker.Extensions;
using TheBugTracker.Models;
using TheBugTracker.Models.Enums;
using TheBugTracker.Models.ViewModels;
using TheBugTracker.Services;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTLookupService _lookupService;
        private readonly IBTRolesService _rolesService;
        private readonly IBTProjectService _projectService;
        private readonly IBTFileService _fileService;
		private readonly IBTCompanyInfoService _companyInfoService;

		public ProjectsController(ApplicationDbContext context, UserManager<BTUser> userManager, IBTLookupService lookupService, IBTRolesService rolesService, IBTProjectService projectService, IBTFileService fileService, IBTCompanyInfoService companyInfoService)
		{
			_context = context;
			_userManager = userManager;
			_lookupService = lookupService;
			_rolesService = rolesService;
			_projectService = projectService;
			_fileService = fileService;
			_companyInfoService = companyInfoService;
		}

		#region // GET: Projects
		// GET: Projects
		public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Projects.Include(p => p.Company).Include(p => p.ProjectPriority);
            return View(await applicationDbContext.ToListAsync());
        }
        #endregion

        #region // GET: Projects/Details/5
        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            int companyId = User.Identity.GetCompanyId().Value;

            Project project = await _projectService.GetProjectByIdAsync(id.Value, companyId) ;

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        #endregion

        #region // GET: Projects/Create
        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Create()
        {
            
            int companyId = User.Identity.GetCompanyId().Value;

            // Add ViewModel instance
            AddProjectWithPMViewModel model = new();

            // Load SelectLists with data, i.e. PMList & PriorityList
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(Roles.ProjectManager.ToString(), companyId),
                         "Id", "FullName");
            model.PriorityList = new SelectList(await _lookupService.GetProjectPrioritiesAsync(), "Id", "Name");

            return View(model);
        }
        #endregion

        #region // POST: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProjectWithPMViewModel model)
        {
            if (model != null)
            {

                int companyId = User.Identity.GetCompanyId().Value;

                try
                {
                    // Add project image - NEED FIX
                    if (model.Project.ImageFormFile != null)
                    {
                        model.Project.ImageFileData = await _fileService.ConvertFileToByteArrayAsync(model.Project.ImageFormFile);
                        model.Project.ImageFileName = model.Project.ImageFormFile.FileName;
                        model.Project.ImageContentType = model.Project.ImageFormFile.ContentType;
                    }

                    // model.Project.Archived = false;
                    // Add companyId
                    model.Project.CompanyId = companyId;

                    await _projectService.AddNewProjectAsync(model.Project);

                    if (!String.IsNullOrEmpty(model.PmId))
                    {
                        await _projectService.AddProjectManagerAsync(model.PmId, model.Project.Id);
                    }

                    // TODO - Redirect to All Projects
                    return RedirectToAction("AllProjects");

                }
                catch (Exception)
                {
                    throw;
                }

            }

            return RedirectToAction("Create");

        }

        #endregion

        #region // GET: Projects/Edit/5
        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            int companyId = User.Identity.GetCompanyId().Value;

            // Add ViewModel instance
            AddProjectWithPMViewModel model = new();

            model.Project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (model.Project == null)
            {
                return NotFound();
            }

            // Load SelectLists with data, i.e. PMList & PriorityList
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(Roles.ProjectManager.ToString(), companyId),
                         "Id", "FullName");
            model.PriorityList = new SelectList(await _lookupService.GetProjectPrioritiesAsync(), "Id", "Name");

            return View(model);
        }
        #endregion

        #region // POST: Projects/Edit/5
        // POST: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddProjectWithPMViewModel model)
        {
            if (model != null)
            {
                try
                {
                    if (model.Project.ImageFormFile != null)
                    {
                        model.Project.ImageFileData = await _fileService.ConvertFileToByteArrayAsync(model.Project.ImageFormFile);
                        model.Project.ImageFileName = model.Project.ImageFormFile.FileName;
                        model.Project.ImageContentType = model.Project.ImageFormFile.ContentType;
                    }

                    await _projectService.UpdateProjectAsync(model.Project);
                  
                    // Add PM if one was chosen
                    if (!string.IsNullOrEmpty(model.PmId))
                    {
                        await _projectService.AddProjectManagerAsync(model.PmId, model.Project.Id);
                    }
                   
                    return RedirectToAction(nameof(AllProjects));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProjectExists(model.Project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
               
            }

            return RedirectToAction("Edit");
        }

        #endregion

        #region // GET: Projects/Archive/5
        // GET: Projects/Archive/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null  )
            {
                return NotFound();
            }

            int companyId = User.Identity.GetCompanyId().Value;

            Project project = await  _projectService.GetProjectByIdAsync( id.Value , companyId );

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        #endregion

        #region // POST: Projects/Archive/5
        // POST: Projects/Archive/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            int companyId = User.Identity.GetCompanyId().Value;

            Project project = await _projectService.GetProjectByIdAsync(id, companyId);
           
            await _projectService.ArchiveProjectAsync(project);

            return RedirectToAction(nameof(AllProjects));
        }
        #endregion

        #region // GET: Projects/Restore/5
        // GET: Projects/Restore/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int companyId = User.Identity.GetCompanyId().Value;
            var project = await _projectService.GetProjectByIdAsync(id.Value, companyId);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        #endregion

        #region // POST: Projects/Restore/5
        // POST: Projects/Restore/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            int companyId = User.Identity.GetCompanyId().Value;
            var project = await _projectService.GetProjectByIdAsync(id, companyId);

            await _projectService.RestoreProjectAsync(project);

            return RedirectToAction(nameof(AllProjects));
        }
        #endregion

        #region //GET: MyProject
        //GET: MyProject
        public async Task<IActionResult> MyProject()
        {
            BTUser bTUser = new();
            List<Project> projets = new();

            bTUser = await _userManager.GetUserAsync(User);
            projets = await _projectService.GetUserProjectsAsync(bTUser.Id);

            return View(projets);
        }
		#endregion

		#region //GET: AllProject
		//GET: MyProject
		public async Task<IActionResult> AllProjects()
		{
            int companyId = User.Identity.GetCompanyId().Value;
			List<Project> projets = new();

			if (User.IsInRole(nameof(Roles.Admin)) || User.IsInRole(nameof(Roles.ProjectManager)))
			{
                projets = await _companyInfoService.GetAllProjectsAsync(companyId);
			}
            else
            {
				projets = await _projectService.GetAllProjectsByCompanyAsync(companyId);
			}

			return View(projets);
		}
		#endregion

		#region //GET: ArchivedProject
		//GET: ArchivedProject
		public async Task<IActionResult> ArchivedProject()
		{
			int companyId = User.Identity.GetCompanyId().Value;

			List<Project> projets = new();

			projets = await _projectService.GetArchivedProjectsByCompany(companyId);

			return View(projets);
		}
        #endregion

        #region //GET: UnassignedProjects
        //GET: MyProject
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnassignedProjects()
        {
            int companyId = User.Identity.GetCompanyId().Value;
            List<Project> projets = new();

            projets = await _projectService.GetUnassignedProjectsAsync(companyId);

            return View(projets);
        }
        #endregion

        #region  // GET: Projects/AssignPM
        [Authorize(Roles = "Admin")]
        [HttpGet]
        // GET: Projects/AssignPM
        public async Task<IActionResult> AssignPM(int projectId)
        {
            int companyId = User.Identity.GetCompanyId().Value;

            AssignPMViewModel model = new();

            model.Project = await _projectService.GetProjectByIdAsync(projectId, companyId);
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(Roles.ProjectManager), companyId), "Id", "FullName");

            return View(model);
        }
        #endregion

        #region // POST: Projects/AssignPM
        // POST: Projects/AssignPM
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignPM(AssignPMViewModel model)
        {

            if (!string.IsNullOrEmpty(model.PMId))
            {
                try
                {
                    await _projectService.AddProjectManagerAsync(model.PMId, model.Project.Id);

                    return RedirectToAction(nameof(Details), new { id = model.Project.Id });
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return RedirectToAction(nameof(AssignPM), new { projectId = model.Project.Id });
        }
        #endregion

        #region  // GET: Projects/AssignMembers
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpGet]
        // GET: Projects/AssignMembers
        public async Task<IActionResult> AssignMembers(int projectId)
        {
            ProjectMembersViewModel model = new();

            int companyId = User.Identity.GetCompanyId().Value;

            model.Project = await _projectService.GetProjectByIdAsync(projectId, companyId);

            List<BTUser> developers = await _rolesService.GetUsersInRoleAsync(nameof(Roles.Developer), companyId);
            List<BTUser> submitters = await _rolesService.GetUsersInRoleAsync(nameof(Roles.Submitter), companyId);

            List<BTUser> companyMembers = developers.Concat(submitters).ToList();
            List<string> projectMembers = model.Project.Members.Select(m => m.Id).ToList();

            // projectMembers will be greyed out in list below
            model.Users = new MultiSelectList(companyMembers, "Id", "FullName", projectMembers);

            return View(model);
        }
        #endregion

        #region // POST: Projects/AssignMembers
        // POST: Projects/AssignMembers
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignMembers(ProjectMembersViewModel model)
        {
            if (model.SelectedUsers != null)
            {
                List<string> memberIds = (await _projectService.GetAllProjectMembersExceptPMAsync(model.Project.Id))
                                                                .Select(m => m.Id).ToList();

                // Remove current members from project
                foreach (string member in memberIds)
                {
                    await _projectService.RemoveUserFromProjectAsync(member, model.Project.Id);
                }

                // Add selected members to project
                foreach (string member in model.SelectedUsers)
                {
                    await _projectService.AddUserToProjectAsync(member, model.Project.Id);
                }

                // Go to project details
                return RedirectToAction("Details", "Projects", new { id = model.Project.Id });
            }

            return RedirectToAction(nameof(AssignMembers), new { projectId = model.Project.Id });
        }

        #endregion

        #region ProjectExists
        private async Task<bool> ProjectExists(int id)
        {
            int companyId = User.Identity.GetCompanyId().Value;

            return (await _projectService.GetAllProjectsByCompanyAsync(companyId)).Any(p => p.Id == id);
        } 
        #endregion
    }
}
