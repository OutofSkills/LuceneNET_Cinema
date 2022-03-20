using LuceneExample;



var results = new List<Movie>();

var movieIndexer = new Cinema();
var luceneIndexer = new LuceneIndexer(@"C:\Users\kojoc\source\repos\LuceneExample\LuceneExample\Index\");

try
{
    luceneIndexer.ClearIndex();
    luceneIndexer.BuildCompleteIndex(movieIndexer.GetDocuments());
}catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

var term = "Simpsons";
var query_results = luceneIndexer.Search(term, "description", 1000);

foreach (var movie in query_results)
{
    results.Add(new Movie { Id = Convert.ToInt32(movie.Get("id")), Content = movie.Get("description") });
    Console.WriteLine(movie.Get("id"));
}

Console.ReadLine();
