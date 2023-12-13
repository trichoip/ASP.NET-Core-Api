using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.Client.MVC.Controllers;

[Route("Store")]
[Route("[controller]")]
[Route("api/[controller]")]
public class RouteController : Controller
{
    private List<Speaker> Speakers =
                 new List<Speaker>
            {
             new Speaker {SpeakerId = 10},
              new Speaker {SpeakerId = 11},
               new Speaker {SpeakerId = 12}
        };

    public IActionResult Index() => View(Speakers);

    [Route("Speaker/{id:int}")]
    public string Detail(int id)
    {
        return "/Route/Speakers Speaker/{id:int} " + Speakers.FirstOrDefault(a => a.SpeakerId == id).SpeakerId;
    }

    [Route("/Speaker/Evaluations", Name = "speakerevals")]
    public string Evaluations()
    {
        return "This is the Route speakerevals /Speaker/Evaluations ";
    }

    [Route("/Speaker/EvaluationsCurrent", Name = "speakerevalscurrent")]
    public string Evaluations(int speakerId, bool currentYear)
    {
        return $"speakerevalscurrent /Speaker/EvaluationsCurrent: speakerId = {speakerId}  || currentYear = {currentYear}";
    }

    [Route("/Home/Test", Name = "Custom")]
    public string Test()
    {
        return "This is the test page";
    }

    [Route("/Home/Feature", Name = "Feature")]
    public string Test2()
    {
        return "This is the test page Feature";
    }

    public void OnGetProfile(int attendeeId)
    {
        ViewData["AttendeeId"] = attendeeId;

        // code omitted for brevity
    }

}

public class Speaker
{
    public int SpeakerId { get; set; }
}
