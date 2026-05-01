using IncidentAPI_imen.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncidentAPI_imen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private static readonly List<Incident> _incidents = new();
        private static int _nextId = 1;

        private static readonly string[] AllowedSeverities =
        { "LOW", "MEDIUM", "HIGH", "CRITICAL" };

        private static readonly string[] AllowedStatuses =
        { "OPEN", "IN_PROGRESS", "RESOLVED" };

        // CREATE INCIDENT
        [HttpPost("create-incident")]
        public IActionResult CreateIncident([FromBody] Incident incident)
        {
            if (incident == null || string.IsNullOrWhiteSpace(incident.Severity))
            {
                return BadRequest("Severity is required.");
            }

            if (!AllowedSeverities.Contains(incident.Severity.ToUpper()))
            {
                return BadRequest(
                    $"Severity must be one of the following: {string.Join(", ", AllowedSeverities)}"
                );
            }

            incident.Id = _nextId++;
            incident.CreatedAt = DateTime.Now;
            incident.Status = "OPEN";

            _incidents.Add(incident);
            return Ok(incident);
        }

        // GET ALL INCIDENTS
        [HttpGet("get-all")]
        public IActionResult GetAllIncidents()
        {
            return Ok(_incidents);
        }

        // GET INCIDENT BY ID
        [HttpGet("getbyid/{id}")]
        public IActionResult GetIncidentById(int id)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            if (incident == null)
            {
                return NotFound("Incident not found");
            }

            return Ok(incident);
        }

        // UPDATE INCIDENT STATUS
        [HttpPut("update-status/{id}")]
        public IActionResult UpdateIncidentStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Status is required");
            }

            var incident = _incidents.FirstOrDefault(i => i.Id == id);
            if (incident == null)
            {
                return NotFound("Incident not found");
            }

            if (!AllowedStatuses.Contains(status.ToUpper()))
            {
                return BadRequest(
                    $"Status must be one of the following: {string.Join(", ", AllowedStatuses)}"
                );
            }

            incident.Status = status.ToUpper();
            return Ok(incident);
        }

        // DELETE INCIDENT
        [HttpDelete("delete-incident/{id}")]
        public IActionResult DeleteIncident(int id)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);
            if (incident == null)
            {
                return NotFound("Incident not found");
            }

            if (incident.Severity.ToUpper() == "CRITICAL" &&
                incident.Status.ToUpper() == "OPEN")
            {
                return BadRequest(
                    "A CRITICAL incident with status OPEN cannot be deleted"
                );
            }

            _incidents.Remove(incident);
            return NoContent();
        }
        [HttpGet("filter-by-status/{statusPart}")]
        public IActionResult FilterByStatus(string statusPart)
        {
            if (string.IsNullOrWhiteSpace(statusPart))
            {
                return BadRequest("Status part is required");
            }

            var results = _incidents
                .Where(i => i.Status != null &&
                            i.Status.Contains(statusPart, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(results);
        }
        [HttpGet("filter-by-Severity/{severityPart}")]
        public IActionResult FilterBySeverity(string severityPart)
        {
            if (string.IsNullOrWhiteSpace(severityPart))
            {
                return BadRequest("Status part is required");
            }

            var results = _incidents
                .Where(i => i.Severity != null &&
                            i.Severity.Contains(severityPart, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(results);
        }


    }
}
