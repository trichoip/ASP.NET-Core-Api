using AspNetCore.TaskScheduler.Data;
using AspNetCore.TaskScheduler.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.TaskScheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IBackgroundJobClient backgroundJobClient;

        public HangfireController(DataContext context, IBackgroundJobClient backgroundJobClient)
        {
            this.context = context;
            this.backgroundJobClient = backgroundJobClient;
        }

        [HttpPost("create")]
        public ActionResult Create(string personName)
        {
            //backgroundJobClient.Enqueue(() => Console.WriteLine(personName));
            backgroundJobClient.Enqueue<IPeopleRepository>(repository => repository.CreatePerson(personName));
            return Ok();
        }

        [HttpPost("schedule")]
        public ActionResult Schedule(string personName)
        {
            var jobId = backgroundJobClient.Schedule(() =>
                Console.WriteLine("The name is " + personName), TimeSpan.FromSeconds(5));

            backgroundJobClient.ContinueJobWith(jobId,
                () => Console.WriteLine($"The job {jobId} has finished"));

            return Ok();
        }

        //public async Task CreatePerson(string personName)
        //{
        //    Console.WriteLine($"Adding person {personName}");
        //    var person = new Person { Name = personName };
        //    context.Add(person);
        //    await Task.Delay(5000);
        //    await context.SaveChangesAsync();
        //    Console.WriteLine($"Added the person {personName}");
        //}

    }
}