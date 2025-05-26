using EventSourcingDemo.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;

namespace EventSourcingDemo.Integration.Test
{
    public class ReservationApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ReservationApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task Can_Seed()
        {
            var start = DateTime.UtcNow.Date;
            var end = DateTime.UtcNow.AddDays(2).Date;
            var tablesResponse = await _client.GetAsync($"/api/Table?start={start:O}&end={end:O}");
            tablesResponse.EnsureSuccessStatusCode();
            var tables = await tablesResponse.Content.ReadFromJsonAsync<List<JsonElement>>();

            if (tables != null && tables.Count == 0)
            {
                // Seed de database
                var seedResponse = await _client.PostAsync("/api/Table/seed", null);
                seedResponse.EnsureSuccessStatusCode();
            }

        }
        [Fact]
        public async Task Can_Create_Reservation()
        {
            // Maak een reservatie voor tafel 3
            var reservation = new
            {
                TableId = 3,
                Name = "howest",
                DateTime = DateTime.UtcNow.AddDays(1),
                NrOfGuests = 6
            };
            var createResponse = await _client.PostAsJsonAsync("/api/Reservation/create-reservation", reservation);
            createResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Then_Is_Reservation_Added_To_Table()
        {
            // Haal alle tafels op
            var start = DateTime.UtcNow.Date;
            var end = DateTime.UtcNow.AddDays(2).Date;
            var tablesResponse = await _client.GetAsync($"/api/Table?start={start:O}&end={end:O}");
            tablesResponse.EnsureSuccessStatusCode();
            var tables = await tablesResponse.Content.ReadFromJsonAsync<List<JsonElement>>();

            Assert.NotNull(tables);
            var table3 = tables.FirstOrDefault(t => t.GetProperty("id").GetInt32() == 3);
            var reservations = table3.GetProperty("reservations").EnumerateArray();
            Assert.Contains(reservations, r =>
                r.GetProperty("name").GetString() == "howest" &&
                r.GetProperty("nrOfGuests").GetInt32() == 6
            );
        }

        [Fact]
        public async Task Plaats_Een_Order()
        {
            // Haal alle tafels op
            var start = DateTime.UtcNow.Date;
            var end = DateTime.UtcNow.AddDays(2).Date;
            var tablesResponse = await _client.GetAsync($"/api/Table?start={start:O}&end={end:O}");
            tablesResponse.EnsureSuccessStatusCode();
            var tables = await tablesResponse.Content.ReadFromJsonAsync<List<JsonElement>>();

            Assert.NotNull(tables);
            var table3 = tables.FirstOrDefault(t => t.GetProperty("id").GetInt32() == 3);
            var reservations = table3.GetProperty("reservations").EnumerateArray();
            var reservation = reservations.FirstOrDefault();
            var reservationIdStr = reservation.ValueKind != JsonValueKind.Undefined ? reservation.GetProperty("reservationId").GetString() : null;
            Assert.False(string.IsNullOrEmpty(reservationIdStr));
            var reservationId = Guid.Parse(reservationIdStr);

            // Plaats een order
            var order = new
            {
                ReservationId = reservationId,
                Order = new
                {
                    OrderId = Guid.NewGuid(),
                    ProductName = "Zeven Zonden Avaritia",
                    ProductId = 5668,
                    Quantity = 2,
                    Price = 3.5,
                    Comment = ""
                }
            };
            var orderResponse = await _client.PostAsJsonAsync("/api/Reservation/order-drinks", order);
            orderResponse.EnsureSuccessStatusCode();
        }
    }
}
