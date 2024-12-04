using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POE.Models;
using System.Diagnostics;
using POE.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Reflection.Metadata;
using iTextSharp.text.pdf;
using iTextSharp.text;  // Ensure this is at the top of your file

using System;
using System.IO;
using System.Linq;


namespace POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Part2dbContext _context;

        public HomeController(ILogger<HomeController> logger, Part2dbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult LectureDetails(Guid id)
        {
            // Retrieve the lecturer by ID from the database
            var lecturer = _context.Lecturers.FirstOrDefault(c => c.LecturerId == id);

            // If lecturer not found, return a 404 Not Found response
            if (lecturer == null)
            {
                return NotFound("The specified lecturer could not be found.");
            }

            // Pass the lecturer object to the view
            return View(lecturer);
        }
        [HttpGet]
        public IActionResult UpdateLecture(Guid? id)
        {
            // Fetch all lecturers for dropdown
            ViewBag.LecturerId = new SelectList(_context.Lecturers, "LecturerId", "Name");

            if (id.HasValue)
            {
                var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == id);
                if (lecturer == null)
                {
                    return NotFound("Lecturer not found.");
                }
                return View(lecturer);
            }

            // Return an empty model if no specific lecturer is selected
            return View(new Lecturer());
        }
        [HttpGet]
        public IActionResult ManageLecturers(string searchTerm)
        {
            var lecturers = _context.Lecturers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                lecturers = lecturers.Where(l => l.Name.Contains(searchTerm) || l.Email.Contains(searchTerm));
            }

            return View(lecturers.ToList());
        }



        [HttpPost]
        public IActionResult UpdateLecture(Lecturer updatedLecturer)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedLecturer);
            }

            var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == updatedLecturer.LecturerId);
            if (lecturer == null)
            {
                // Log the error or print the ID for debugging
                Console.WriteLine($"Lecturer not found with ID: {updatedLecturer.LecturerId}");
                return NotFound("Lecturer not found.");
            }

            // Update fields
            lecturer.Name = updatedLecturer.Name;
            lecturer.Email = updatedLecturer.Email;
            lecturer.ContactNumber = updatedLecturer.ContactNumber;
            lecturer.Address = updatedLecturer.Address;

            _context.SaveChanges();
            return RedirectToAction("LectureDetails", new { id = updatedLecturer.LecturerId });
        }




        public IActionResult Lecture()
        {
            return View();
        }

        // Handle form submission and registration logic
        [HttpPost]
        public async Task<IActionResult> Lecture(Lecturer lecturer)
        {
            // Check if the model is valid based on data annotations in the Lecturer model
            if (ModelState.IsValid)
            {
                // Add the new lecturer to the database
                _context.Lecturers.Add(lecturer);
                await _context.SaveChangesAsync();

                // Redirect to a success page or another action, e.g., "Details" page for the lecturer
                return RedirectToAction("LectureDetails", new { id = lecturer.LecturerId });
            }

            // If model validation fails, return the form with validation errors
            return View(lecturer);
        }


        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(LecturerClaim claim, IFormFile? document)
        {
            if (ModelState.IsValid)
            {
                if (document != null && document.Length > 0)
                {
                    // Restrict file size (5 MB limit)
                    if (document.Length > 5242880)
                        return BadRequest("File size too large.");

                    // Restrict file type
                    var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                    var extension = Path.GetExtension(document.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        return BadRequest("Invalid file type. Only .pdf, .docx, and .xlsx are allowed.");
                    }

                    // Define the path where the file will be saved
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + document.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                    }

                    // Store the relative path in DocumentPath
                    claim.DocumentPath = Path.Combine("documents", uniqueFileName);
                }

                // Set the submission date and status
                claim.SubmissionDate = DateTime.Now;
                claim.Status = "Pending";

                // Save the claim to the database
                _context.LecturerClaims.Add(claim);
                await _context.SaveChangesAsync();

                // Redirect to the ClaimSubmitted action, passing the claim ID
                return RedirectToAction("Claims", new { id = claim.Id });
            }

            return View(claim);
        }

        [HttpGet]
        public IActionResult Claims(Guid id)
        {
            // Fetch the claim details by ID
            var claim = _context.LecturerClaims.FirstOrDefault(c => c.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            // Pass the claim object to the view
            return View(claim);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexHR()
        {
            return View();
        }
        public IActionResult ApprovedClaims()
        {
            // Retrieve all claims with the status "Approved"
            var approvedClaims = _context.LecturerClaims.Where(c => c.Status == "Approved").ToList();

            // Pass the approved claims to the view
            return View(approvedClaims);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AdminUsers objUser)
        {
            var obj = _context.adminUsers.Where(a => a.UserNames.Equals(objUser.UserNames) && a.UserPassword.Equals(objUser.UserPassword)).FirstOrDefault();
            if (obj != null)
            {
                HttpContext.Response.Cookies.Append("Authenticated", "True");
                return RedirectToAction("CoordinatorClaimVerificationView");
            }
            return View();
        }
        public IActionResult LoginHR()
        {
            var claims = _context.LecturerClaims.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult LoginHR(AdminUsers objUser)
        {
            var obj = _context.adminUsers.Where(a => a.UserNames.Equals(objUser.UserNames) && a.UserPassword.Equals(objUser.UserPassword)).FirstOrDefault();
            if (obj != null)
            {
                HttpContext.Response.Cookies.Append("Authenticated", "True");
                return RedirectToAction("IndexHR");
            }
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(AdminUsers model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save the user data to the database
                    using (var context = new Part2dbContext())
                    {
                        context.adminUsers.Add(model);
                        context.SaveChanges();
                    }

                    // Redirect to the Login page after successful registration
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    // Log the exception and display an error message
                    ModelState.AddModelError("", "An error occurred while registering the user. Please try again.");
                }
            }

            // If we reach this point, something went wrong, re-display the form
            return View(model);
        }
        public IActionResult SignUpHR()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUpHR(AdminUsers model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save the user data to the database
                    using (var context = new Part2dbContext())
                    {
                        context.adminUsers.Add(model);
                        context.SaveChanges();
                    }

                    // Redirect to the Login page after successful registration
                    return RedirectToAction("LoginHR");
                }
                catch (Exception ex)
                {
                    // Log the exception and display an error message
                    ModelState.AddModelError("", "An error occurred while registering the user. Please try again.");
                }
            }

            // If we reach this point, something went wrong, re-display the form
            return View(model);
        }



        [HttpPost]
        public IActionResult ApproveClaim(Guid id)
        {
            var claim = _context.LecturerClaims.Find(id);
            if (claim != null)
            {
                claim.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("ApprovedClaims");
        }
        public IActionResult RejectedClaims()
        {
            // Retrieve all claims with the status "Rejected"
            var rejectedClaims = _context.LecturerClaims.Where(c => c.Status == "Rejected").ToList();

            // Pass the rejected claims to the view
            return View(rejectedClaims);
        }


        [HttpPost]
        public IActionResult RejectClaim(Guid id)
        {
            var claim = _context.LecturerClaims.Find(id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("RejectedClaims");
        }

        public IActionResult CoordinatorClaimVerificationView()
        {
            var claims = _context.LecturerClaims.Where(c => c.Status == "Pending").ToList();
            return View(claims);
        }

        [HttpGet]
        public IActionResult AppeoveRejectedClaims()
        {
            // Fetch rejected claims from the database
            var rejectedClaims = _context.LecturerClaims
                                         .Where(c => c.Status == "Rejected")
                                         .ToList();
            return View(rejectedClaims);
        }

        [HttpPost]
        public IActionResult AppeoveRejectedClaims(Guid claimId)
        {
            // Find the claim by ID
            var claim = _context.LecturerClaims.FirstOrDefault(c => c.Id == claimId);
            if (claim == null)
            {
                // Handle claim not found
                return NotFound("Claim not found.");
            }

            // Update the claim status to 'Approved'
            claim.Status = "Approved";
            _context.SaveChanges();

            // Redirect back to the Rejected Claims view
            return RedirectToAction("ApprovedClaims");
        }
        [HttpGet]
        public IActionResult RejectApprovedClaims()
        {
            // Fetch rejected claims from the database
            var rejectedClaims = _context.LecturerClaims
                                         .Where(c => c.Status == "Approved")
                                         .ToList();
            return View(rejectedClaims);
        }

        [HttpPost]
        public IActionResult RejectApprovedClaims(Guid claimId)
        {
            // Find the claim by its ID
            var claim = _context.LecturerClaims.FirstOrDefault(c => c.Id == claimId);
            if (claim == null)
            {
                return NotFound("Claim not found.");
            }

            // Mark the claim as rejected (again)
            claim.Status = "Rejected"; // This status can remain unchanged or updated for re-rejection tracking.
            _context.SaveChanges();

            // Redirect back to the Rejected Claims view
            return RedirectToAction("RejectedClaims");
        }


        [HttpGet]
        public IActionResult GenerateInvoice()
        {
            // Fetch all approved claims from the database
            var approvedClaims = _context.LecturerClaims.Where(c => c.Status == "Approved").ToList();

            if (approvedClaims == null || !approvedClaims.Any())
            {
                return NotFound("No approved claims found.");
            }

            // Create the PDF in a MemoryStream
            using (var stream = new MemoryStream())
            {
                try
                {
                    // Use iTextSharp's Document class (fully qualified)
                    var document = new iTextSharp.text.Document(PageSize.A4);
                    var writer = PdfWriter.GetInstance(document, stream);
                    writer.CloseStream = false; // Keep stream open after document is closed

                    // Open the document for writing content
                    document.Open();

                    // Add title to the document
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    document.Add(new Paragraph("Invoice for Approved Claims", titleFont));
                    document.Add(new Paragraph(" ")); // Add an empty line for spacing

                    // Iterate through each approved claim and add its details to the document
                    foreach (var approvedClaim in approvedClaims)
                    {
                        // Add claim details to the document
                        document.Add(new Paragraph($"Claim ID: {approvedClaim.Id}"));
                        document.Add(new Paragraph($"Lecturer Name: {approvedClaim.LecturerName}"));
                        document.Add(new Paragraph($"Hours Worked: {approvedClaim.HoursWorked}"));
                        document.Add(new Paragraph($"Hourly Rate: {approvedClaim.HourlyRate:C}"));
                        document.Add(new Paragraph($"Total Amount: {approvedClaim.TotalAmount:C}"));
                        document.Add(new Paragraph($"Submission Date: {approvedClaim.SubmissionDate:dd MMM yyyy}"));

                        if (!string.IsNullOrEmpty(approvedClaim.Notes))
                        {
                            document.Add(new Paragraph($"Notes: {approvedClaim.Notes}"));
                        }

                        // Add some space between each claim
                        document.Add(new Paragraph(" ")); // Empty line for spacing between claims
                    }

                    // Close the document after adding all content
                    document.Close();

                    // Reset the stream position before returning
                    stream.Position = 0;

                    // Return the PDF as a file response
                    return File(stream.ToArray(), "application/pdf", "ApprovedClaimsInvoice.pdf");
                }
                catch (Exception ex)
                {
                    // Handle any exceptions and provide a meaningful error message
                    return StatusCode(500, $"Error generating invoice: {ex.Message}");
                }
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
