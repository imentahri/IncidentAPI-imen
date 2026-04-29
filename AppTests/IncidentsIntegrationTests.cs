using IncidentAPI_imen.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
namespace AppTests
{
    public class IncidentsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public IncidentsIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetIncidents_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/IncidentsDb");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"GET failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
            }
        }

        [Fact]
        public async Task PostIncident_CreatesIncident()
        {
            var incident = new
            {
                Title = "Test Incident",
                Description = "Test Description",
                Severity = "HIGH",
                CreatedAt = DateTime.UtcNow
            };

            var response = await _client.PostAsJsonAsync("/api/IncidentsDb", incident);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"POST failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
            }

            var createdIncident = await response.Content.ReadFromJsonAsync<Incident>();

            Assert.NotNull(createdIncident);
            Assert.Equal("Test Incident", createdIncident.Title);
            Assert.Equal("HIGH", createdIncident.Severity);
        }

        [Fact]
        public async Task PostThenGet_ReturnsInsertedIncident()
        {
            var incident = new
            {
                Title = "Integration Test",
                Description = "Test Description",
                Severity = "MEDIUM",
                CreatedAt = DateTime.UtcNow
            };

            var postResponse = await _client.PostAsJsonAsync("/api/IncidentsDb", incident);

            if (!postResponse.IsSuccessStatusCode)
            {
                var body = await postResponse.Content.ReadAsStringAsync();
                throw new Exception($"POST failed: {(int)postResponse.StatusCode} {postResponse.StatusCode}. Body: {body}");
            }

            var response = await _client.GetAsync("/api/IncidentsDb");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"GET failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
            }

            var data = await response.Content.ReadFromJsonAsync<List<Incident>>();

            Assert.Contains(data, i => i.Title == "Integration Test");
        }
    }
}