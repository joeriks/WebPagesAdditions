##WebPagesAdditions.BoundWebPage

ToDo: suggest a better name...

Greatly simplifies to build json api's in a WebPages project

**Basic usage**

In any WebPages project:

	install-package WebPagesAdditions.BoundWebPage

Add a folder named /api

Add a resource in that folder named people.cshtml

Add the following code:

	@inherits WebPagesAdditions.BoundWebPage
	@* The HTML is only returned for non matching calls for data *@

	<h1>Some information</h1>

	<a href="/api/people/1234">Get item # 1234</a>
	<a href="/api/people/dosomething">Call "dosomething"</a>

	@functions{

		// returns json data on the url /api/people/:id (int)
		public object GET(int id){

			var someObject = new { id = id, data = "whatever" };
			return someObject;
           
		}

		// returns data on the url /api/people/dosomething 
		public object dosomething_GET() {

			return "done something";
           
		}
	}

now a call to /api/people/123 returns
	{id:123,data:"whatever"}

where a call to /api/people returns the html

and a call to /api/people/dosomething returns
	done something

**Data binding**

	@inherits WebPagesAdditions.BoundWebPage

	<form method="post">

		<input name="Id" value="@person.Id"/>
		<input name="Age" value="@person.Age"/>
		<input name="Name" value="@person.Name"/>
		<input type="submit" />

	</form>

	<a href="/api/people/dosomething/@person.Id">do it</a>

	@status

	@functions{

		// define the viewmodel within the script itself

		Person person;
		string status = "";

		class Person
		{
			public int Id { get; set; }        
			[System.ComponentModel.DataAnnotations.Required]
			public string Name { get; set; }        
			public int Age { get; set; }

		}
    
		// for all requests
		public void init()
		{
			person = Bind(new Person());
		}

		// runs only if the url is /:id (int) and method = get
		public object GET(int id)
		{
			// Bind is using the default asp net data binding
			person = Bind(new Person { Id = id });
			return person;
		}

		// runs only if the url is / and method = post
		public void POST()
		{
			if (ModelState.IsValid)
			{
				// post to repo        
				status = "Valid post";
			} else {
				status = "Invalid post, check binder for more info :-p";
			}
		}

	}

** Function naming convnetions **

init() runs first on all requests

then the functions should be named:

1) by http method (upper case)
2) by resource a underscore and http method

with signatures with integer and or string parameters

GET() responds to GET requests to resource root
POST() responds to POST requests to resource root

anything_GET() responds to GET requests to /anything

GET(id:int) responds to GET requests to resource root /:id (int)
GET(id:string) responds to GET requests to resource root /:id (string)

