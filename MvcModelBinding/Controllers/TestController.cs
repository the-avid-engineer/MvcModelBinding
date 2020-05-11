using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MvcModelBinding.Controllers
{
    // For [ApiController]
    // As far as MVC is concerned, an explicitly undecorated controller method parameter is implicitly decorated with [FromBody]
    // There can only be a single [FromBody] parameter; [FromHeader], [FromQuery], [FromForm], and [FromRoute] bindings are ignored on the [FromBody] parameter
    // [ApiController] (short for [ApiControllerAttribute]) adds behavior, such as automatically returning (405/Bad Request) if any of the model binding is invalid
    //[ApiController]

    // For [Controller]
    // As far as MVC is concerened, there are no implicit decorations and I'm unaware of it making any assumptions about your controller method parameters.
    // There is a package you can add to make this behave the "same" way as MVC in .NET Framework, only used on Field App API
    // [Controller] (short for [ControllerAttribute]) is inherited by default from the Controller class so you dont *have* to add it but I like to add it anyway.
    //[Controller]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        #region Test Models for FromHeader, FromQuery, FromForm

        public class UndecoratedModel
        {
            public string test1 { get; set; }
            public string test2 { get; set; }
            public string[] test3 { get; set; }
        }

        public class HeaderDecoratedModel
        {
            [FromHeader]
            public string test1 { get; set; }

            [FromHeader(Name = "Test-Header")]
            public string test2 { get; set; }

            [FromHeader]
            public string[] test3 { get; set; }
        }

        public class QueryDecoratedModel
        {
            [FromQuery]
            public string test1 { get; set; }

            [Required]
            [FromQuery(Name = "test-query")]
            public string test2 { get; set; }

            [FromQuery]
            public string[] test3 { get; set; }
        }

        public class FormDecoratedModel
        {
            [FromForm]
            public string test1 { get; set; }

            [FromForm(Name = "Test-Form")]
            public string test2 { get; set; }

            [FromForm]
            public string[] test3 { get; set; }
        }

        #endregion

        #region [FromHeader]

        #region Style A (Always Works)

        /// <summary>
        /// Endpoint Info Goes Here
        /// </summary>
        /// <param name="test1">Parameter Info for test1 Goes Here</param>
        /// <param name="test2">Parameter Info for test2 Goes Here</param>
        /// <param name="test3">Parameter Info for test3 Goes Here</param>
        /// <returns>Return Info Goes Here</returns>
        [HttpGet("HeaderA")]
        [ProducesResponseType(typeof(UndecoratedModel), (int)HttpStatusCode.OK)]
        public IActionResult Header
        (
            [FromHeader] string test1,
            [FromHeader(Name = "Test-Header")] string test2,
            [FromHeader] string[] test3
        )
        {
            return Ok(new UndecoratedModel
            {
                test1 = test1,
                test2 = test2,
                test3 = test3,
            });
        }

        #endregion

        #region Style B (Never Works)

        [HttpGet("HeaderB")]
        public IActionResult Header
        (
            [FromHeader] UndecoratedModel header
        )
        {
            return Ok(header);
        }

        #endregion

        #region Style C (Only works with [ApiController] if the controller method parameter also has [FromHeader])

        [HttpGet("HeaderC")]
        public IActionResult Header
        (
            /*[FromHeader]*/ HeaderDecoratedModel header
        )
        {
            return Ok(header);
        }

        #endregion

        #endregion

        #region [FromQuery]

        #region Style A (Always Works)

        [HttpGet("QueryA")]
        public IActionResult Query
        (
            [FromQuery] string test1,
            [FromQuery(Name = "test-query")] string test2,
            [FromQuery] string[] test3
        )
        {
            return Ok(new
            {
                test1,
                test2,
                test3,
            });
        }

        #endregion

        #region Style B (Always Works)

        [HttpGet("QueryB")]
        public IActionResult Query
        (
            [FromQuery] UndecoratedModel query
        )
        {
            return Ok(query);
        }

        #endregion

        #region Style C (Only works with [ApiController] if the controller method parameter also has [FromQuery])

        [HttpGet("QueryC")]
        public IActionResult Query
        (
            /*[FromQuery]*/ QueryDecoratedModel query
        )
        {
            return Ok(query);
        }

        #endregion

        #endregion

        #region [FromForm]

        // Style A - bind each form field to one parameter of the controller methods

        #region Style A (Always Works)

        [HttpGet("FormA")]
        public IActionResult Form
        (
            [FromForm] string test1,
            [FromForm(Name = "Test-Form")] string test2,
            [FromForm] string[] test3
        )
        {
            return Ok(new
            {
                test1,
                test2,
                test3,
            });
        }

        #endregion

        // Style B - bind the entire set of form fields to an undecorated model

        #region Style B (Always Works)

        [HttpGet("FormB")]
        public IActionResult Form
        (
            [FromForm] UndecoratedModel form
        )
        {
            return Ok(form);
        }

        #endregion

        // Style C - bind the entire set of form fields to a decorated model

        #region Style C (Only works with [ApiController] if the controller method parameter also has [FromForm])

        [HttpGet("FormC")]
        public IActionResult Form
        (
            /*[FromForm]*/ FormDecoratedModel form
        )
        {
            return Ok(form);
        }

        #endregion

        #endregion



        #region Test Models for FromRoute

        public class UndecoratedRouteModel
        {
            public string routeId { get; set; }
        }

        public class DecoratedRouteModel
        {
            [FromRoute]
            public string routeId { get; set; }
        }

        #endregion

        #region FromRoute

        #region Style A

        [HttpGet("RouteA1/{routeId}")]
        public IActionResult RouteIdRequired
        (
            [FromRoute] string routeId
        )
        {
            return Ok(routeId);
        }

        [HttpGet("RouteA2/{routeId?}")]
        public IActionResult RouteIdOptional
        (
            [FromRoute] string routeId
        )
        {
            return Ok(routeId);
        }

        [HttpGet("RouteA3/{routeId?}")]
        public IActionResult RouteIdOptionalDefault
        (
            [FromRoute(Name = "routeId")] string someRandomName = "ABC"
        )
        {
            return Ok(someRandomName);
        }

        #endregion

        #region Style B

        [HttpGet("RouteB1/{routeId}")]
        public IActionResult RouteIdRequired
        (
            [FromRoute] UndecoratedRouteModel route
        )
        {
            return Ok(route);
        }

        #endregion

        #region Style C

        [HttpGet("RouteC1/{routeId}")]
        public IActionResult RouteIdRequired
        (
            /*[FromRoute]*/ DecoratedRouteModel route
        )
        {
            return Ok(route);
        }

        #endregion

        #endregion



        #region Test Models for FromBody

        public class UndecoratedBodyModel
        {
            public string test1 { get; set; }
            public string test2 { get; set; }
            public string[] test3 { get; set; }
        }

        #endregion

        #region FromBody

        // body.GetType() == typeof(JsonElement)
        // will accept any valid json and serialize it

        [HttpPost("BodyA")]
        public IActionResult Body
        (
            [FromBody] object body
        )
        {
            return Ok(body);
        }

        // will only accept json that conforms to UndecoratedModel

        [HttpPost("BodyB")]
        public IActionResult Body
        (
            [FromBody] UndecoratedBodyModel body
        )
        {
            return Ok(body);
        }

        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.formatters.textinputformatter?view=aspnetcore-3.1

        #endregion
    }
}
