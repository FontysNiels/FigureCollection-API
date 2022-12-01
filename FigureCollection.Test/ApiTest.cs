using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FigureCollection.Classes;
using FigureCollection.Controllers;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.VisualBasic;
using Microsoft.Extensions.DependencyInjection;
using FigureCollection.Data;

namespace FigureCollection.Test
{
    [TestClass]
    public class ApiTest
    {
        private HttpClient _httpClient;

        Figure NewFigure = new Figure()
        {
            id = 1,
            name = "Integration Test",
            Brand = new Brand()
            {
                id = 2
            },
            Manufacturer = new Manufacturer()
            {
                id = 2
            },
            Character = new Character()
            {
                id = 2
            },
            Line = new Line()
            {
                id = 1
            },
            Edition = new Edition()
            {
                id = 3
            },
            size = 0,
            scale = 0
        };
        public static int NewestFigureId;
        public ApiTest()
        {
            //var webAppFactory = new WebApplicationFactory<Program>();
            //_httpClient = webAppFactory.CreateDefaultClient();
            //CreateClient

            var application = new FakeProgram();

            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var notesDbContext = provider.GetRequiredService<DataContext>())
                {
                    notesDbContext.Database.EnsureCreatedAsync();

                    notesDbContext.Figures.AddAsync(NewFigure);
                    
                    notesDbContext.SaveChangesAsync();
                }
            }

            _httpClient = application.CreateClient();
            
        }

        [TestMethod]
        public async Task A_PostFigure()
        {
            NewFigure.id = 2;
            NewFigure.name = "Integration Test NOT standard";
            var content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(NewFigure)));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PostAsync("/api/Figures", content);
            string stringResult = await response.Content.ReadAsStringAsync();

            List<Figure> AllFigures = JsonConvert.DeserializeObject<List<Figure>>(stringResult);
            Figure LastAddedFigure = AllFigures[AllFigures.Count-1];
            NewestFigureId = AllFigures[AllFigures.Count-1].id;

            Assert.AreEqual(LastAddedFigure.name, NewFigure.name);
        }

        [TestMethod]
        public async Task B_GetFigureOnId()
        {
            var response = await _httpClient.GetAsync("/api/Figures/" + 1);
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(stringResult.Contains("Integration Test"));
        }

        [TestMethod]
        public async Task C_PutFigure()
        {
            NewFigure.id = 1;
            NewFigure.name = "Integration Test Put";

            var content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(NewFigure)));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.PutAsync("/api/Figures", content);
            string stringResult = await response.Content.ReadAsStringAsync();

            List<Figure> AllFigures = JsonConvert.DeserializeObject<List<Figure>>(stringResult);
            Figure LastAddedFigure = AllFigures[AllFigures.Count - 1];

            Assert.AreNotEqual("Integration Test", LastAddedFigure.name);
        }

        [TestMethod]
        public async Task D_DeleteFigure()
        {
            var response = await _httpClient.DeleteAsync("/api/Figures/" + 1);
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.IsFalse(stringResult.Contains("Integration Test Put"));
        }

        [TestMethod]
        public async Task E_DeleteNonExistingFigure()
        {
            var response = await _httpClient.DeleteAsync("/api/Figures/" + 888);
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(stringResult.Contains("Figure Not Found."));
        }
    }
}