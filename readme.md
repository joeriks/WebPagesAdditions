##WebPagesAdditions.BoundWebPage

ToDo: suggest a better name...

Greatly simplifies to build json api's in a WebPages project

**Sample:**

Add a folder named /api

Add a resource in that folder named persons.cshtml

Add the following code:

	@inherits WebPagesAdditions.BoundWebPage
	<h1>Some information</h1>

	<a href="/api/persons/1234">Get item # 1234</a>
	<a href="/api/persons/dosomething">Call "dosomething"</a>

	@functions{

		// returns data on the url /api/persons/:id (int)
		public object GET(int id){

			var someObject = new { id = id, data = "whatever" };
			return someObject;
           
		}

		// returns data on the url /api/persons/dosomething 
		public object dosomething_GET() {

			return "done something";
           
		}
	}

Check sample site for more samples